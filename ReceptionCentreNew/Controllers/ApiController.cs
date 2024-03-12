using ReceptionCentreNew.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ReceptionCentreNew.Controllers
{
    public class ApiController : Controller
    {
        public static string token = "YDh57mCasOfVDavjUd-YjI4y9HLK-w5FPmvF1LWsKmmG1egfxkLrPUsZ9kP9fzKDdz_Tl1pUyD3mY-yMoX5eLsIGa7vcWKDOIhMycCcYBeUCCvfxKibaCNgcHFBlL1PDvi5HtKL3TSmTp9ympelN0fE57vJOohTAInavBTZMwMPx9vZMBIHQ00CtBDhahelgy7AQfatYyCb-d_mXAZFMmVymdNyhFHBLIWEejlD31vEnSipPwHKJLJB4S1BwEzfH-za5ZDHgamEQHXaSL0i0sZQVS9ZqScXRcfNhOJ1kbX5hNBHBkSHfaex6Erytz1w4tMo44nlIPA0VjzPGE7_aV5_u9rR9qHmPqBLQLLLl4I1b24TQrHsUzE62jFqzbeIUtdYGl5EjqFLUG3xELvtvdnnl9v1HcIkAKmh2gTsCuZy3Vqde00tdLilvHTTXoIGw";
        public static string GetToken()
        {
            var url = string.Format("http://192.168.34.9:8090/Token");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            request.Method = "POST";
            string postParameters = "grant_type=password&username=bega-m@yandex.ru&password=!15963210Ap";
            byte[] bytes = Encoding.UTF8.GetBytes(postParameters);
            request.ContentLength = bytes.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream_rez = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream_rez);

            string data = reader.ReadToEnd();

            reader.Close();
            stream_rez.Close();
            Token model_token = JsonConvert.DeserializeObject<Token>(data);
            token = model_token.AccessToken;
            return token;
        }
        public string GetHttpClient(string uri)
        {
            string rez = "";
            try
            {
                var url = string.Format("http://192.168.34.9:8090/api/v1/" + uri);
                HttpWebRequest request = HttpWebRequest.CreateHttp(url);
                request.Headers.Add("Authorization", "Bearer " + token);
                request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                rez = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }
            catch (Exception e)
            {
                rez = e.Message;
            }
            return rez;
        }
        public string GetResponseApi(string uri)
        {
            string result;
            var http_result = GetHttpClient(uri);
            if (http_result.Contains("(401)")) // если отказано в авторизации
            {
                GetToken();
                result = GetHttpClient(uri);
            }
            else
            {
                result = http_result;
            }
            return result;
        }

        #region Данные по услуге
        public IActionResult GetServicesInfo(Guid id)
        {
            var msgInfo = GetResponseApi("Services/" + id);
            var msgDocuments = GetResponseApi("Services/" + id + "/Documents/1912196");
            var msgFailure = GetResponseApi("Services/" + id + "/Failure");
            var msgFailureDocuments = GetResponseApi("Services/" + id + "/FailureDocuments");
            var msgResult = GetResponseApi("Services/" + id + "/Result");
            var msgStop = GetResponseApi("Services/" + id + "/Stop");
            var msgTariff = GetResponseApi("Services/" + id + "/Tariff/1912196");
            var msgWay = GetResponseApi("Services/" + id + "/Way");
            var msgWayResult = GetResponseApi("Services/" + id + "/WayResult");

            ServicesSubInfo model = new ServicesSubInfo
            {
                Info = msgInfo != "[]" ? msgInfo != "null" ? JsonConvert.DeserializeObject<List<ServicesSubInfo>>(msgInfo) : new List<ServicesSubInfo>() : new List<ServicesSubInfo>(),
                Documents = msgDocuments != "[]" ? msgDocuments != "null" ? JsonConvert.DeserializeObject<List<ServicesSubDocuments>>(msgDocuments) : new List<ServicesSubDocuments>() : new List<ServicesSubDocuments>(),
                Failure = msgFailure != "[]" ? msgFailure != "null" ? JsonConvert.DeserializeObject<List<ServicesSubFailure>>(msgFailure) : new List<ServicesSubFailure>() : new List<ServicesSubFailure>(),
                FailureDocuments = msgFailureDocuments != "[]" ? msgFailureDocuments != "null" ? JsonConvert.DeserializeObject<List<ServicesSubFailure>>(msgFailureDocuments) : new List<ServicesSubFailure>() : new List<ServicesSubFailure>(),
                Result = msgResult != "[]" ? msgResult != "null" ? JsonConvert.DeserializeObject<List<ServicesSubResult>>(msgResult) : new List<ServicesSubResult>() : new List<ServicesSubResult>(),
                Stop = msgStop != "[]" ? msgStop != "null" ? JsonConvert.DeserializeObject<List<ServicesSubStop>>(msgStop) : new List<ServicesSubStop>() : new List<ServicesSubStop>(),
                Tariff = msgTariff != "[]" ? msgTariff != "null" ? JsonConvert.DeserializeObject<List<ServicesSubTariff>>(msgTariff) : new List<ServicesSubTariff>() : new List<ServicesSubTariff>(),
                Way = msgWay != "[]" ? msgWay != "null" ? JsonConvert.DeserializeObject<List<ServicesSubWay>>(msgWay) : new List<ServicesSubWay>() : new List<ServicesSubWay>(),
                WayResult = msgWayResult != "[]" ? msgWayResult != "null" ? JsonConvert.DeserializeObject<List<ServicesSubWay>>(msgWayResult) : new List<ServicesSubWay>() : new List<ServicesSubWay>(),
            };

            return PartialView("ServicesInfo", model);
        }
        #endregion

        #region Хештеги
        public IActionResult GetHashtag(string search)
        {
            ViewBag.Search = search;
            if (search != null && search.Replace(" ","") != "")
            {                
                var msg = GetResponseApi("Services/SearchByHashtag/1912196/" + search.TrimStart('#').Replace('#', ';'));
                var Services = JsonConvert.DeserializeObject<List<HashtagServicesList>>(msg);                
                return PartialView("Hashtags", Services);
            }
            else
            {
                return PartialView("Hashtags", new List<HashtagServicesList>());
            }

        }
        #endregion

        #region Заявители
        public IActionResult GetCustomers(string PhoneNumber)
        {
            try
            {
                string pn = PhoneNumber;
                if (PhoneNumber != null)
                {
                    if (PhoneNumber.Length == 11 && PhoneNumber.Substring(0, 1) == "8")
                    {
                        pn = "7" + PhoneNumber.TrimStart('8');
                    }

                    ViewBag.PhoneNumber = PhoneNumber;
                    var msg = GetResponseApi("Jitsi/SearchCustomer/" + pn.Replace(" ", ""));
                    var Customers = JsonConvert.DeserializeObject<ServicesSubCustomerResponse>(msg);
                    var model = Customers;
                    return PartialView("Customers", model);
                }
                else
                {
                    return PartialView("Customers", new ServicesSubCustomerResponse());
                }

            }
            catch
            {
                return PartialView("Customers", new ServicesSubCustomerResponse());
            }
        }
        #endregion

        #region Услуги у заявителя
        public IActionResult GetCustomerCases(string snils)
        {
            if (snils != null)
            {
                ViewBag.Snils = snils;
                var msg = GetResponseApi("Case/" + snils);
                var UserInfo = JsonConvert.DeserializeObject<List<AccountCase>>(msg);
                return PartialView("Cases", UserInfo.OrderByDescending(o=>o.DateEnter).Take(3));
            }
            else
            {
                return PartialView("Cases", new List<AccountCase>());
            }

        }
        #endregion

    }
}