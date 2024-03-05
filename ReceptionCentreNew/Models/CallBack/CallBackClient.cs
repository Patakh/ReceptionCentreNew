using Newtonsoft.Json;
using static ReceptionCentreNew.Models.ModelsCallBack;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Data.Context.App;
using System.Text;

namespace ReceptionCentreNew.Models;
public class CallBackClient
{
    static string ServiceUrl { get; set; }
    static string AuthCode { get; set; }

    public CallBackClient(string host, string version, string query)
    {
        ServiceUrl = host + version + query;
    }
    private bool ActiveMode
    {
        get
        {
            using (ReceptionCentreContext db = new())
            {
                bool act = Convert.ToBoolean(db.SprSetting.FirstOrDefault(f => f.Id == 51)?.ParamValue);
                return act;
            }
        }
    }

    public async Task<Response> RefreshCallBack()
    {
        if (!ActiveMode)
            return null;
        try
        {
            string url = ServiceUrl;
            var query = new CallBackList().Value;
            int errorCount = 0;
            int notSyncCount = 0;
            int processCount = 0;
            int updateCount = 0;
            int syncCount = 0;
            List<CallBackConfirmItem> ConfirmList = new();
            // для пометки добавленных в базу записей
            string urlFlag = "http://192.168.34.191:3838/rest/confirmOrdered";
            string flagMethod = "POST";
            using (HttpClient client = new())
            {
                query = JsonConvert.DeserializeObject<CallBackList>(await client.GetStringAsync(url))?.Value.Where(w => w.Ip == "192.168.34.254").ToList();
            }
            using (ReceptionCentreContext db = new())
            {
                var saveList = query.Select(s => new CallBackQuery { BaseId = null, DateOrder = s.Date, PhoneNumber = s.Phone, Ip = s.Ip, CallbackId = s.Id })
                    .OrderBy(o => o.DateOrder);
                // если вдруг не удастся отметить, что звонки сохранились в АИС
                var notSyncList = db.DataCallback.Where(w => w.IsSync == false).ToList();
                var notSyncConfirm = notSyncList.Select(s => new CallBackConfirmItem { Id = s.CallbackId });
                notSyncCount = notSyncList.Count();
                var ApiController = new Controllers.ApiController();
                foreach (var item in saveList)
                {
                    try
                    {
                        string msgResult = ApiController.GetResponseApi("Jitsi/SearchCustomer/" + item.PhoneNumber.Replace(" ", ""));
                        var Result = msgResult != "[]" ? msgResult != null ? msgResult != "Удаленный сервер возвратил ошибку: (404) Не найден." ? JsonConvert.DeserializeObject<ResponseCustomer>(msgResult) : new ResponseCustomer() : new ResponseCustomer() : new ResponseCustomer();
                        var fio = Result?.Customers.FirstOrDefault()?.CustomerFio;
                        DataCallback model = new()
                        {
                            CountTry = 0,
                            CustomerFio = fio ?? "Заявитель не найден",
                            DateOrder = item.DateOrder,
                            PhoneNumber = item.PhoneNumber,
                            Status = 1,
                            CallbackId = item.CallbackId,
                            IsSync = false,
                        };
                        if (db.DataCallback.Where(w => w.PhoneNumber == item.PhoneNumber && w.Status == 1).Count() == 0)//только если нет активных заявок
                        {
                            db.DataCallback.Add(model);
                            db.SaveChanges();
                            updateCount++;
                        }
                        ConfirmList.Add(new CallBackConfirmItem { Id = model.CallbackId });
                    }
                    catch (Exception ex)
                    {
                        string m = ex.Message;
                        errorCount++;
                    }
                    processCount++;
                }
                if (ConfirmList.Count() > 0)
                {
                    try
                    {
                        using (HttpClient client = new())
                        {
                            var data = JsonConvert.SerializeObject(ConfirmList);
                            var content = new StringContent(data, Encoding.UTF8, "application/json");
                            var response = await client.PostAsync(urlFlag, content);
                            if (response.IsSuccessStatusCode)
                            {
                                var responseContent = await response.Content.ReadAsStringAsync();
                                var responseObject = JsonConvert.DeserializeObject<CallBackList>(responseContent);
                                if (responseObject.Status == "ok")
                                {
                                    var arr = ConfirmList.Select(s => s.Id).ToArray();
                                    var callbacks = db.DataCallback.Where(w => arr.Contains(w.CallbackId)).ToList();
                                    callbacks.ForEach(f => f.IsSync = true);
                                    await db.SaveChangesAsync();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string m = ex.Message;
                    }
                }
                if (notSyncConfirm.Count() > 0)
                {
                    try
                    {
                        using (HttpClient client = new())
                        {
                            string data = JsonConvert.SerializeObject(notSyncConfirm);
                            var content = new StringContent(data, Encoding.UTF8, "application/json");
                            var response = await client.PostAsync(urlFlag, content);
                            if (response.IsSuccessStatusCode)
                            {
                                var responseContent = await response.Content.ReadAsStringAsync();
                                var responseObject = JsonConvert.DeserializeObject<CallBackList>(responseContent);
                                if (responseObject.Status == "ok")
                                {
                                    var arr = notSyncConfirm.Select(s => s.Id).ToArray();
                                    var callbacks = db.DataCallback.Where(w => arr.Contains(w.CallbackId)).ToList();
                                    callbacks.ForEach(f => f.IsSync = true);
                                    await db.SaveChangesAsync();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string m = ex.Message;
                        errorCount++;
                    }
                }
            }
            return new Response()
            {
                AllCount = query.Count() + notSyncCount,
                ProcessCount = processCount,
                ErrorCount = errorCount,
                UpdateCount = updateCount,
                NotSyncCount = notSyncCount,
                SyncCount = syncCount,
            };

        }
        catch (Exception ex)
        {
            string m = ex.Message;
            return null;
        }
    }
}