using ReceptionCentreNew.Filters; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using ReceptionCentreNew.Data.Context.App;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using System.Text;
using System.Security.Cryptography;
//using OpenPop.Mime;
//using OpenPop.Pop3;
using ReceptionCentreNew.Data.Context.App.Abstract;
using System.Data;
using ReceptionCentreNew.Models;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Imap;
using MailKit;
using MimeKit;
using Newtonsoft.Json;
using MailKit.Security;
using MailKit.Search;

namespace ReceptionCentreNew.Controllers
{
    [ClientErrorHandler]
    [Authorize]
    public class CommonController : Controller
    { 
        private IRepository _repository;
        private string? UserName;
        public SignInManager<ApplicationUser> SignInManager;
        public CommonController(IRepository repo, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            SignInManager = signInManager;
            _repository = repo; 
            UserName = _repository.SprEmployees.First(s => s.EmployeesLogin == signInManager.Context.User.Identity.Name).EmployeesName;
        } 

        private static string keyCrypt = ConfigurationManager.AppSettings["CryptKey"];
        public class Resultt {
            public int FreeEmails { get; set; }
            public int NewEmail { get; set; }
            public int FreeSms { get; set; }
            public int FreeCalls { get; set; }
            public int MyNotifications { get; set; }
        }
        public class CountResult
        {
            public int CallsInDay { get; set; }
            public int CallsInWeek { get; set; }
            public int CallsInMonth { get; set; }
            public int CallsInDayAnswer { get; set; }
            public int CallsInDayMissed { get; set; }

            public IEnumerable<string> CallsInDayMissedList { get; set; }
        }
        public class EmailCredentials
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string ImapServer { get; set; }
            public int ImapPort { get; set; }
        }


        public async Task<JsonResult> ListenerAsync()
        {
            SprEmployees employees = _repository.SprEmployees.Where(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name).FirstOrDefault();
            Resultt rez = new();

            var setting = _repository.SprSetting;
            string email_login = setting.FirstOrDefault(w => w.ParamName == "email_login")?.ParamValue;
            string email_password = setting.FirstOrDefault(w => w.ParamName == "email_password")?.ParamValue;

            using (var client = new ImapClient())
            {
                await client.ConnectAsync("imap.yandex.ru", 993, true); // Подключение к IMAP серверу
                await client.AuthenticateAsync(email_login, email_password); // Аутентификация

                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadWrite); // Открытие почтового ящика

                var uids_from_base = _repository.DataAppealEmail.Select(s => s.Uids).ToList();
               int  max_ = uids_from_base.Max() != null ? int.Parse(uids_from_base.Max()) : 0;

                var uniqueIds = await inbox.SearchAsync(SearchQuery.OlderThan(max_));
                foreach (var uniqueId in uniqueIds)
                {
                    var message = await inbox.GetMessageAsync(uniqueId); // Получение сообщения

                    DataAppealEmail model = new()
                    {
                        DateEmail =DateTime.Parse(message.Date.ToString("yyyy-MM-dd HH:mm:ss")),
                        EmployeesNameAdd = "Admin",
                        SprEmployeesId = Guid.Parse("3155fc8f-7bd4-4ba9-97e4-969c3ccd5a58"),
                        Uids = uniqueId.Id.ToString(),
                        Email = message.From.Mailboxes.First()?.Address,
                        Caption = message.Subject,
                        TextEmail = message.TextBody,
                        EmailType = 2,
                        IsRead = false
                    };

                    try
                    {
                        model.TextEmail = string.IsNullOrEmpty(model.TextEmail) ? "-" : model.TextEmail;
                        await _repository.Insert(model);
                        rez.NewEmail += 1;
                    }
                    catch (Exception e)
                    {
                        string ee = e.Message;
                    }

                    uids_from_base.Add(uniqueId.Id.ToString());
                }

                await client.DisconnectAsync(true); // Отключение от сервера
            }

            return Json(rez);
        }

        public async Task<IActionResult> GetNotifications()
        {
            SprEmployees employees = await _repository.SprEmployees.Where(se => se.EmployeesLogin ==SignInManager.Context.User.Identity.Name).FirstOrDefaultAsync();
            var appeal = _repository.DataEmployeesNotification.Where(w => w.SprEmployeesId == employees.Id && w.IsActive == true);
            if (await appeal.AnyAsync())
                return Json(appeal.CountAsync());
            return Json(0);
        }

        public async Task<IActionResult> GetEmails()
        {
            SprEmployees employees = await _repository.SprEmployees.Where(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name).FirstOrDefaultAsync();

            var appeal = _repository.DataAppealEmail.Where(w => w.DataAppealId == null);
            if (await appeal.AnyAsync()) 
               return Json(appeal.CountAsync());
            return Json(0);
        }
        public async Task<IActionResult> GetCalls()
        { 
            var appeal = _repository.DataAppealCall.Where(w => w.DataAppealId == null);
            if (await appeal.AnyAsync())
                return Json(appeal.CountAsync());
            return Json(0);
        }
        public async Task<IActionResult> GetCallsParameters()
        {
            DateTime datDay = DateTime.Parse(DateTime.Now.ToShortDateString());
            DateTime datWeek = DateTime.Now.AddDays(-7);
            DateTime datMonth = DateTime.Now.AddMonths(-1);
            List<string> b =await _repository.DataAppealCall.Where(w => w.DateCall > datDay && w.TimeTalk != null).Select(s => s.PhoneNumber).ToListAsync();
            List<string> c =await _repository.DataAppealCall.Where(w => w.DateCall > datDay && w.TimeTalk == null).Select(s => s.PhoneNumber).ToListAsync();
            IEnumerable<string> missed_list = c.Except(b);
            CountResult model = new()
            {
                CallsInDay = await _repository.DataAppealCall.Where(w => w.DateCall > datDay).CountAsync(),
                CallsInWeek = await _repository.DataAppealCall.Where(w => w.DateCall > datWeek).CountAsync(),
                CallsInMonth = await _repository.DataAppealCall.Where(w => w.DateCall > datMonth).CountAsync(),
                CallsInDayAnswer = await _repository.DataAppealCall.Where(w => w.DateCall > datDay && w.TimeTalk != null).Select(s => s.PhoneNumber).CountAsync(),
                CallsInDayMissed = missed_list.Count(),
                CallsInDayMissedList = missed_list
            };
            return Json(model);
        }        

        public IActionResult Play_Audio_File(Guid Id)
        {
            ViewBag.FileId = Id;
            return PartialView("../Common/Play_Audio_File");
        }

        public IActionResult Get_Audio_File(Guid Id)
        {
            DataAppealFile file = _repository.DataAppealFile.Where(w => w.Id == Id).FirstOrDefault();
            Response.AppendTrailer("Accept-Ranges", "bytes");
            var settings = _repository.SprSetting.ToList();
            string ftpServer = settings.SingleOrDefault(ss => ss.ParamName == "ftp_server")?.ParamValue;
            string ftpFolder = settings.SingleOrDefault(ss => ss.ParamName == "ftp_folder_files")?.ParamValue;
            string ftpLogin = settings.SingleOrDefault(ss => ss.ParamName == "ftp_user")?.ParamValue;
            string ftpPass = CRPassword.Encrypt(settings.SingleOrDefault(ss => ss.ParamName == "ftp_password")?.ParamValue);
            FtpFileModel ftp = new();
            byte[] songbyte = ftp.OpenURI(ftpServer, ftpLogin, ftpPass, "/RECEPTION/" + ftpFolder, Id + file.FileExt);

            FileExtensionContentTypeProvider provider = new ();
            if (!provider.TryGetContentType(Id + file.FileExt, out string typeExt))
            {
                typeExt = "application/octet-stream"; // MIME тип по умолчанию, если не удается определить для файла
            } 

            return base.File(songbyte, typeExt);
        }

        [HttpPost]
        public string GetFile(Guid Id)
        {
            DataAppealFile file = _repository.DataAppealFile.Where(w => w.Id == Id).FirstOrDefault();
            Response.AppendTrailer("Accept-Ranges", "bytes");
            var settings = _repository.SprSetting.ToList();
            string ftpServer = settings.SingleOrDefault(ss => ss.ParamName == "ftp_server")?.ParamValue;
            string ftpFolder = settings.SingleOrDefault(ss => ss.ParamName == "ftp_folder_files")?.ParamValue;
            string ftpLogin = settings.SingleOrDefault(ss => ss.ParamName == "ftp_user")?.ParamValue;
            string ftpPass = CRPassword.Encrypt(settings.SingleOrDefault(ss => ss.ParamName == "ftp_password")?.ParamValue);
            FtpFileModel ftp = new();
            byte[] Filebyte = ftp.OpenURI(ftpServer, ftpLogin, ftpPass, "/RECEPTION/" + ftpFolder, Id + file.FileExt);

            if (Filebyte != null)
            {
                string base64String = Convert.ToBase64String(Filebyte, 0, Filebyte.Length);
                return base64String;
            }
            else
            {
                return null;
            }
        }


        /// /// Шифрует строку value
        /// 
        /// Строка которую необходимо зашифровать
        /// Ключ шифрования
        public static string Encrypt(string str)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(str), keyCrypt));
        }
        private static byte[] Encrypt(byte[] key, string value)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            PasswordDeriveBytes passwordDeriveBytes = new(value, key);
            ICryptoTransform ct = sa.CreateEncryptor(key, passwordDeriveBytes.GetBytes(16)); 

            MemoryStream ms = new();
            CryptoStream cs = new(ms, ct, CryptoStreamMode.Write);

            cs.Write(key, 0, key.Length);
            cs.FlushFinalBlock();

            byte[] Result = ms.ToArray();

            ms.Close();
            ms.Dispose();

            cs.Close();
            cs.Dispose();

            cs.Dispose();

            return Result;
        }
        public static string DeCrypt(string Hash)
        {
            var _key = ConfigurationManager.AppSettings["CryptKey"];
            try
            {
                var Pass = Decrypt(Hash, _key);
                return Pass;
            }
            catch
            {
                return null;
            }
        }
        public static string Decrypt(string str, string keyCrypt)
        {
            string Result;
            try
            {
                CryptoStream Cs = InternalDecrypt(Convert.FromBase64String(str), keyCrypt);
                StreamReader Sr = new(Cs);

                Result = Sr.ReadToEnd();

                Cs.Close();
                Cs.Dispose();

                Sr.Close();
                Sr.Dispose();
            }
            catch (CryptographicException)
            {
                return null;
            }

            return Result;
        }
        private static CryptoStream InternalDecrypt(byte[] key, string value)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            PasswordDeriveBytes passwordDeriveBytes = new(value, key);
            ICryptoTransform ct = sa.CreateEncryptor(key, passwordDeriveBytes.GetBytes(16));

            MemoryStream ms = new(key);
            return new CryptoStream(ms, ct, CryptoStreamMode.Read);
        }
    }
}