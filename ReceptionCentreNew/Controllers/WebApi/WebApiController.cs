using ReceptionCentreNew.Filters;
using ReceptionCentreNew.Hubs;
using ReceptionCentreNew.Models.WebApi;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReceptionCentreNew.Data.Context.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using ReceptionCentreNew.Data.Context.App.Abstract;

namespace ReceptionCentreNew.Controllers.WebApi;
[ClientErrorHandler]
[AllowAnonymous]
public class WebApiController : ApiController
{
    private IRepository _repository;
    private readonly IHubContext<NotificationHub> _hubContext;
    public WebApiController(IRepository repo, IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
        _repository = repo;
    }

    #region Models
    public class ApiRequest
    {
        public string user { get; set; }
        public Guid? SprMfcId { get; set; }
    }

    public class FtpFile
    {
        public byte[] Filebyte { get; set; }
        public string Name { get; set; }
        public string TimeTalk { get; set; }
        public string Extension { get; set; }
        public string CallType { get; set; }
        public JitsiCallType CallTypeJitsi { get; set; }
    }
    public class ApiJitsiCall
    {
        public string CustomerTel { get; set; }
        public string CustomerName { get; set; }
        public string SprEmployeesId { get; set; }
        public string SetEmployeesFio { get; set; }
        public string DataServicesInfoId { get; set; }
        public string DataServicesId { get; set; }
        public string IpAddress { get; set; }
        public string CallType { get; set; }
        public string UserName { get; set; }
        public string CallId { get; set; }
        public DateTime? DateCall { get; set; }
        public JitsiCallType CallTypeJitsi { get; set; }
    }

    #endregion

    public JsonResult GetClaimStatistics(ApiRequest request)
    {
        try
        {
            ResponseClaimStatistics model = new()
            {
                ClaimStatistics = _repository.FuncDataAppealClaimStatistics(request.SprMfcId),
                ResponseMessage = "OK",
                ErrorCode = 0,
                ErrorMessage = ""
            };
            return Json(model);

        }
        catch (Exception e)
        {
            return Json(new ResponseClaimStatistics { ResponseMessage = "Error", ErrorCode = e.HResult, ErrorMessage = e.InnerException?.Message ?? e.Message });
        }
    }
    public JsonResult GetClaimWeek(ApiRequest request)
    {
        try
        {
            ResponseClaimWeek model = new()
            {
                ClaimWeek = _repository.FuncDataAppealClaimWeek(request.SprMfcId),
                ResponseMessage = "OK",
                ErrorCode = 0,
                ErrorMessage = ""
            };
            return Json(model);
        }
        catch (Exception e)
        {
            return Json(new ResponseClaimWeek { ResponseMessage = "Error", ErrorCode = e.HResult, ErrorMessage = e.InnerException?.Message ?? e.Message });
        }
    }
    public JsonResult GetClaimYear(ApiRequest request)
    {
        try
        {
            ResponseClaimYear model = new()
            {
                ClaimYear = _repository.FuncDataAppealClaimYear(request.SprMfcId),
                ResponseMessage = "OK",
                ErrorCode = 0,
                ErrorMessage = ""
            };
            return Json(model);
        }
        catch (Exception e)
        {
            return Json(new ResponseClaimYear { ResponseMessage = "Error", ErrorCode = e.HResult, ErrorMessage = e.InnerException?.Message ?? e.Message });
        }
    }

    #region Методы для учета и загрузки звонков
    /// <summary>
    /// Запись звонка
    /// </summary>
    /// <param name="modal">Данные о звонке</param>
    /// <returns></returns>
    public JsonResult SaveCall(ApiJitsiCall modal)
    {
        try
        {
            if (ModelState.IsValid)
                return Json(new { ResponseCode = 0, ResponseMessage = "Ok", Id = ApiJitsiSaveCallId(modal) });
            else
                return Json(new { ResponseCode = 1, ResponseMessage = String.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(s => s.ErrorMessage)) });
        }
        catch (Exception e)
        {
            return Json(new { ResponseCode = 1, ResponseMessage = e.InnerException?.Message ?? e.Message });
        }
    }

    /// <summary>
    /// Загрузка звонка
    /// </summary>
    /// <param name="file">Файл для записи звонка</param>
    /// <returns></returns>
    public JsonResult UploadCall(FtpFile file)
    {
        try
        {
            if (ModelState.IsValid)
                return Json(new { ResponseCode = 0, ResponseMessage = "Ok", Flag = api_jitsi_upload_call(file) });
            else
                return Json(new { ResponseCode = 1, ResponseMessage = String.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(s => s.ErrorMessage)) });
        }
        catch (Exception e)
        {
            return Json(new { ResponseCode = 1, ResponseMessage = e.InnerException?.Message ?? e.Message });
        }
    }

    public Guid? ApiJitsiSaveCallId(ApiJitsiCall model)
    {
        string path = @"C:\inetpub\wwwroot\ais_reception_jitsi\Content\log\log-savecall.txt";
        try
        {
            // Обычный звонок
            DataAppealCall saveCallModel = new()
            {
                Id = Guid.NewGuid(),
                DateCall = model.DateCall != null ? model.DateCall : DateTime.Now,
                EmployeesNameAdd = model.SetEmployeesFio != null ? model.SetEmployeesFio : "Jitsi",
                CallType = Convert.ToInt16(model.CallType),
                SaveFtp = false,
                PhoneNumber = model.CustomerTel,
            };
            if (model.SprEmployeesId != null)
            {
                saveCallModel.SprEmployeesId = Guid.Parse(model.SprEmployeesId);
            }
            if (model.DataServicesId != null)
            {
                saveCallModel.DataAppealId = Guid.Parse(model.DataServicesId);
            }
            // Обратный звонок
            DataCallbackCalls saveCallBackCallModel = new()
            {
                CallDuration = "00:00:00",
                DateCall = model.DateCall != null ? (DateTime)model.DateCall : DateTime.Now,
                EmployeeFio = model.SetEmployeesFio,
                SaveFtp = false,
            };
            if (model.SprEmployeesId != null)
            {
                var empId = Guid.Parse(model.SprEmployeesId);
                var Employee = _repository.SprEmployees.Where(w => w.Id == empId).FirstOrDefault();
                var depId = Employee?.SprEmployeesDepartmentId;
                var Dep = _repository.SprEmployeesDepartment.Where(w => w.Id == depId).FirstOrDefault();
                saveCallBackCallModel.SprEmployeesId = empId;
                saveCallBackCallModel.DepartmentName = Dep?.DepartmentName;
                saveCallBackCallModel.SprEmployeesDepartmentId = depId.Value;
            }
            if (model.DataServicesId != null)
            {
                saveCallBackCallModel.DataCallbackId = Guid.Parse(model.DataServicesId);
            }

            switch (model.CallTypeJitsi)
            {
                case JitsiCallType.outgoing: // исходящий
                    _repository.Insert(saveCallModel);
                    SignalRReturnCallId(saveCallModel.Id, model.UserName);
                    return saveCallModel.Id;
                case JitsiCallType.incoming: // входящий
                    _repository.Insert(saveCallModel);
                    return saveCallModel.Id;
                case JitsiCallType.outgoing_callback: // обратный
                    _repository.Insert(saveCallBackCallModel);
                    var r = RefreshCallBackStatusAsync(saveCallBackCallModel.DataCallbackId, saveCallBackCallModel.SprEmployeesId);
                    return saveCallBackCallModel.Id;
                default: return null;
            }

        }
        catch (Exception ex)
        {
           Logger.Log(path, $"Error {ex.Message}");
            return null;
        }
    }
    public bool api_jitsi_upload_call(FtpFile file)
    {
        string path = @"C:\inetpub\wwwroot\ais_reception_jitsi\Content\log\log-savecall.txt";
        try
        {
            string status = "No";
            if (file.Filebyte != null)
            {
                var settings = _repository.SprSetting.ToList();
                var ftpModel =
                    new
                    {
                        ftpServer = settings.SingleOrDefault(ss => ss.ParamName == "ftp_server")?.ParamValue,
                        ftpFolder = settings.SingleOrDefault(ss => ss.ParamName == "ftp_folder_calls")?.ParamValue,
                        ftpLogin = settings.SingleOrDefault(ss => ss.ParamName == "ftp_user")?.ParamValue,
                        ftpPass = CRPassword.Encrypt(settings.SingleOrDefault(ss => ss.ParamName == "ftp_password")?.ParamValue)
                    };

                FtpFileModel ftp = new();
                status = ftp.UploadCallFtp(file.Filebyte, ftpModel.ftpServer, ftpModel.ftpLogin, ftpModel.ftpPass, ftpModel.ftpFolder + "/" + file.Name + file.Extension).ToString();
                Guid callId = Guid.Parse(file.Name);
                var upDateCall = _repository.DataAppealCall.Where(w => w.Id == callId).FirstOrDefault();
                var upDateCallBackCall = _repository.DataCallbackCalls.Where(w => w.Id == callId).FirstOrDefault();
                switch (file.CallTypeJitsi)
                {
                    case JitsiCallType.outgoing: // исходящий
                        if (upDateCall != null)
                        {
                            upDateCall.TimeTalk = file.TimeTalk;
                            if (status == "OK")
                                upDateCall.SaveFtp = true;
                            _repository.Update(upDateCall);
                        }
                        break;
                    case JitsiCallType.incoming: // входящий
                        if (upDateCall != null)
                        {
                            upDateCall.TimeTalk = file.TimeTalk;
                            if (status == "OK")
                                upDateCall.SaveFtp = true;
                            _repository.Update(upDateCall);
                        }
                        break;
                    case JitsiCallType.outgoing_callback: // обратный
                        if (upDateCallBackCall != null)
                        {
                            upDateCallBackCall.CallDuration = file.TimeTalk;
                            if (status == "OK")
                                upDateCallBackCall.SaveFtp = true;
                            _repository.Update(upDateCallBackCall);
                        }
                        break;
                    default: break;
                }
                return (status == "OK" ? true : false);
            }
            return false;
        }
        catch (Exception ex)
        {
            Logger.Log(path, $"Error {ex.Message}");
            return false;
        }
    }

    public async Task SignalRReturnCallId(Guid Id, string UserName)
    {
        await _hubContext.Clients.User(Id.ToString()).SendAsync("ReturnCallId", Id);
    }
    #endregion

    #region FROM Jitsi
    /// <summary>
    /// Запись звонка
    /// </summary>
    /// <param name="modal">Данные о звонке</param>
    /// <returns></returns>
    public IActionResult SaveCallV2(JitsiCallsRequest modal)
    {
        Guid employeeId = Guid.Parse(modal.IdEmployee);
        var Employee = modal.IdEmployee != null ? _repository.SprEmployees.Where(w => w.Id == employeeId).FirstOrDefault() : null;
        var model = new ApiJitsiCall
        {
            CallId = modal.Id.ToString(),
            CallType = modal.CallType.ToString() == "incoming" ? "1" : "2",
            CustomerName = modal.CustomerName,
            CustomerTel = modal.CustomerPhone,
            DataServicesInfoId = modal.CaseNumber,
            IpAddress = modal.IpAddress,
            SetEmployeesFio = Employee?.EmployeesName,
            SprEmployeesId = modal.IdEmployee,
            UserName = Employee?.EmployeesLogin,
            DataServicesId = modal.IdService
        };

        try
        {
            if (TryValidateModel(model))
                return Json(new { ResponseCode = 0, ResponseMessage = "Ok", Id = ApiJitsiSaveCallId(model) });
            else
                return Json(new { ResponseCode = 1, ResponseMessage = string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(s => s.ErrorMessage)) });
        }
        catch (Exception e)
        {
            return Json(new { ResponseCode = 1, ResponseMessage = e.InnerException?.Message ?? e.Message });
        }
    }

    /// <summary>
    /// Загрузка звонка
    /// </summary>
    /// <param name="file">Файл для записи звонка</param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult UploadCallV2(JitsiCallFile file)
    {
        FtpFile ftpFile = new FtpFile
        {
            Filebyte = (byte[])(Array)file.Filebyte,
            TimeTalk = file.CallDuration,
            Name = file.Id.ToString(),
            CallType = "1",
            Extension = ".mp3"
        };

        try
        {
            if (TryValidateModel(ftpFile))
            {
                byte[] unsigned = new byte[ftpFile.Filebyte.Length];
                Buffer.BlockCopy(ftpFile.Filebyte, 0, unsigned, 0, ftpFile.Filebyte.Length);
                ftpFile.Filebyte = unsigned;
                return Json(new { ResponseCode = 0, ResponseMessage = "Ok", Flag = api_jitsi_upload_call(ftpFile) });
            }
            else
                return Json(new { ResponseCode = 1, ResponseMessage = String.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(s => s.ErrorMessage)) });
        }
        catch (Exception e)
        {
            return Json(new { ResponseCode = 1, ResponseMessage = e.InnerException?.Message ?? e.Message });
        }
    }

    /// <summary>
    /// Сохранение звонка и загрузка файла на ФТП
    /// </summary>
    /// <param name="modal"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult SaveCallV3()
    {
        var modal = GetModelFromJsonRequest<JitsiCallsUnionRequest>(HttpContext.Request);
        string path = @"C:\inetpub\wwwroot\ais_reception_jitsi\Content\log\log-savecall.txt";
        bool? flag = false;
        Guid? Id = null;
        Guid? employeeId = modal.IdEmployee != null ? Guid.Parse(modal.IdEmployee) : Guid.Empty;
        var Employee = modal.IdEmployee != null ? _repository.SprEmployees.Where(w => w.Id == employeeId).FirstOrDefault() : null;
        var callModel = new ApiJitsiCall
        {
            CallId = modal.Id.ToString(),
            CallType = modal.CallType.ToString() == "outgoing" ? "1" : modal.CallType.ToString() == "incoming" ? "2" : "3",
            CustomerName = modal.CustomerName,
            CustomerTel = modal.CustomerPhone,
            DataServicesInfoId = modal.CaseNumber,
            IpAddress = modal.IpAddress,
            SetEmployeesFio = string.IsNullOrEmpty(modal.EmployeeFio) ? Employee?.EmployeesName : modal.EmployeeFio,
            SprEmployeesId = modal.IdEmployee,
            UserName = Employee?.EmployeesLogin,
            DataServicesId = modal.IdService,
            DateCall = modal.DateCall,
            CallTypeJitsi = modal.CallType
        };
        Logger.Log(path, $"-------------------");
        Logger.Log(path, $"Модель звонка");
        FtpFile fileModel = new FtpFile
        {
            Filebyte = (byte[])(Array)modal.Filebyte,
            TimeTalk = modal.CallDuration,
            Extension = modal.AudioFormat.Contains(".") ? modal.AudioFormat : "." + modal.AudioFormat,
            CallTypeJitsi = modal.CallType
        };
        Logger.Log(path, $"Модель файла звонка для загрузки на ftp");
        try
        {
            if (TryValidateModel(callModel))
            {
                Logger.Log(path, $"Начало записи звонка:");
                Id = ApiJitsiSaveCallId(callModel);
                Logger.Log(path, $"Сохранение записи звонка: с Id - {Id}");
                fileModel.Name = Id.ToString();
                Logger.Log(path, $"Начало загрузки звонка на ФТП:");
                flag = api_jitsi_upload_call(fileModel);
                Logger.Log(path, $"Результат загрузки звонка на ФТП: flag - {flag}");
                //----
                Logger.Log(path, $"ResponseCode = 0, ResponseMessage = Ok, Id = {Id}, Flag = {flag}");
                return Json(new { ResponseCode = 0, ResponseMessage = "Ok", Id = Id, Flag = flag });
            }
            else
            {
                Logger.Log(path, $"ResponseCode = 1, ResponseMessage = {String.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(s => s.ErrorMessage))}, Id = {Id}, Flag = {flag}");
                return Json(new { ResponseCode = 1, ResponseMessage = String.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(s => s.ErrorMessage)), Id = Id, Flag = flag });
            }
        }
        catch (Exception e)
        {
            Logger.Log(path, $"ResponseCode = 2, ResponseMessage = {e.InnerException?.Message ?? e.Message}, Id = {Id}, Flag = {flag}");
            return Json(new { ResponseCode = 2, ResponseMessage = e.InnerException?.Message ?? e.Message, Id = Id, Flag = flag });
        }
        //return RedirectToAction("SaveCall", model);
    }

    public static T GetModelFromJsonRequest<T>(HttpRequest request)
    {
        string result = "";
        request.Body.Seek(0, SeekOrigin.Begin);
        using (StreamReader reader = new(request.Body))
        {
            result = reader.ReadToEnd();
        }
        return JsonConvert.DeserializeObject<T>(result);
    }

    public async Task<bool> RefreshCallBackStatusAsync(Guid Id, Guid? employeeId)
    {
        try
        {
            var model = _repository.DataCallback.Include(i => i.DataCallbackCalls).First(f => f.Id == Id);
            model.CountTry = model.DataCallbackCalls.Count();
            var seconds = 0;
            foreach (var item in model.DataCallbackCalls)
            {
                var t = item.CallDuration.Split(':');

                int _h = int.Parse(t[0]);
                int _m = int.Parse(t[1]);
                int _s = int.Parse(t[2]);
                seconds += (_h * 60 * 60) + (_m * 60) + _s;
            }
            // Статус звонка (1 - Новый звонок, 2 - Обработан, 3 - Не отвеченный)
            int status = model.CountTry > 0 ? seconds > 13 ? 2 : model.CountTry >= 3 ? 3 : 1 : 1;
            model.Status = status;
            if (status == 2 || status == 3) // если заявка обработана указываем кто и когда обработал
            {
                if (employeeId.HasValue)
                {
                    model.SprEmployeesIdClose = Guid.Parse(employeeId.Value.ToString());
                }
                model.DateClose = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            _repository.Update(model);

            using (HttpClient client = new())
            {
                HttpResponseMessage response = await client.GetAsync("http://192.168.34.9:8085/CallBack/signalRCallback?Id=" + employeeId.ToString());
                client.Dispose();
                response.Dispose();
            }
        }
        catch (Exception ex)
        {
            string m = ex.Message;
            return false;
        }
        return true;
    }
    #endregion
}