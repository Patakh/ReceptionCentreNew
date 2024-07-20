using ReceptionCentreNew.Filters;
using ReceptionCentreNew.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Data.Context.App.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Core.Types;

namespace ReceptionCentreNew.Controllers;
[ClientErrorHandler]
[Authorize]
public class CallController : Controller
{
    private IRepository _repository;
    public SignInManager<ApplicationUser> SignInManager;
    public CallController(IRepository repo, SignInManager<ApplicationUser> signInManager)
    {
        SignInManager = signInManager;
        _repository = repo;
    }

    public IActionResult Calls()
    {
        var employees = _repository.SprEmployees.Where(e => e.IsRemove != true);
        if (!User.IsInRole("superadmin") && !User.IsInRole("admin"))
            employees = employees.Where(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);

        ViewBag.SprEmployees = new SelectList(employees, "Id", "EmployeesName");
        return View();
    }
    public IActionResult PartialTableCalls(Guid? SprEmployeeId, short? period, short? type, short? IsConnected)
    {
        DateTime dateStart;
        switch (period)
        {
            case 1: dateStart = DateTime.Now; break;
            case 2: dateStart = DateTime.Now.AddDays(-7); break;
            case 3: dateStart = DateTime.Now.AddMonths(-1); break;
            case 4: dateStart = DateTime.Now.AddYears(-1); break;
            default: dateStart = DateTime.Now.AddYears(-2); break;
        }
        var calls = _repository.FuncDataAppealCallSelect(SprEmployeeId, dateStart, DateTime.Now.AddDays(1), type, IsConnected).ToArray();
        //foreach(var item in calls)
        //{
        //    string phone = item.OutPhoneNumber;
        //    if(phone.Length==11)
        //    {
        //        phone = "+7(" + phone.Substring(1, 3) + ")" + phone.Substring(4, 3) + "-" + phone.Substring(7, 2) + "-" + phone.Substring(9, 2);
        //    }
        //    var appeal = _repository.DataAppeal.Where(w=>w.PhoneNumber==phone || w.PhoneNumber == item.OutPhoneNumber && w.ApplicantName.Length>3  ).FirstOrDefault();
        //    item.ApplicantName = appeal != null ? appeal.ApplicantName:"-";
        //}
        CallsViewModel model = new()
        {
            CallList = calls.OrderByDescending(o => o.OutDateCall),
        };
        return PartialView(model);
    }

    public IActionResult Play_Audio(Guid Id)
    {
        ViewBag.CallId = Id;
        return PartialView("../Common/Play_Audio");
    }
     
    public IActionResult Get_Audio(Guid Id)
    {
        var settings = _repository.SprSetting.ToList();
        string ftpServer = settings.SingleOrDefault(ss => ss.ParamName == "ftp_server")?.ParamValue;
        string ftpFolder = settings.SingleOrDefault(ss => ss.ParamName == "ftp_folder_calls")?.ParamValue;
        string ftpLogin = settings.SingleOrDefault(ss => ss.ParamName == "ftp_user").ParamValue;
        string ftpPass = settings.SingleOrDefault(ss => ss.ParamName == "ftp_password").ParamValue;

        FtpFileModel ftp = new();

        byte[] songbyte = ftp.OpenURI(ftpServer, ftpLogin, ftpPass, "/RECEPTION/" + ftpFolder+"/", Id + ".mp3");

        return File(songbyte, "audio/mp3");
    }

    public IActionResult UserInfo(string PhoneNumber)
    {
        ViewBag.PhoneNumber = PhoneNumber;
        return PartialView("Modal/UserInfo");
    }

    public IActionResult NewAppeal(string PhoneNumber)
    {
        ViewBag.SprCategory = new SelectList(_repository.SprCategory.OrderBy(o => o.CategoryName), "Id", "CategoryName");
        ViewBag.SprType = new SelectList(_repository.SprType, "Id", "TypeName");
        ViewBag.SprTypeDifficulty = new SelectList(_repository.SprTypeDifficulty.OrderBy(o => o.CountDay), "Id", "TypeName");
        ViewBag.SprSubjectTreatment = new SelectList(_repository.SprSubjectTreatment.OrderBy(o => o.SubjectName), "Id", "SubjectName");
        ViewBag.SprMfc = new SelectList(_repository.SprMfc.OrderBy(o => o.MfcName), "Id", "MfcName");

        string format_phone = "";
        if (PhoneNumber.Replace(" ", "").Length == 11)
            format_phone = "+7(" + PhoneNumber.Substring(1, 3) + ")" + PhoneNumber.Substring(4, 3) + "-" + PhoneNumber.Substring(7, 2) + "-" + PhoneNumber.Substring(9, 2);

        var appeal = _repository.DataAppeal.Where(w => w.PhoneNumber == PhoneNumber || w.PhoneNumber == format_phone && w.ApplicantName.Length > 3).FirstOrDefault();
        DataAppeal model = new();
        model.PhoneNumber = PhoneNumber;
        if (appeal != null)
        {
            model.ApplicantName = appeal.ApplicantName;
            model.Email = appeal.Email;
        }
        return PartialView("Modal/NewAppeal", model);
    }
    public string FormatPhoneNumber(string phoneNumber)
    {
        string originalValue = phoneNumber;
        string value = originalValue.TrimStart('8');
        if (phoneNumber.Length == 7)
            return Convert.ToInt64(value).ToString("###-####");
        if (phoneNumber.Length == 9)
            return Convert.ToInt64(value).ToString("###-###-####");
        if (phoneNumber.Length == 10)
            return Convert.ToInt64(value).ToString("###-###-####");
        if (phoneNumber.Length > 10)
        {
            phoneNumber = new System.Text.RegularExpressions.Regex(@"\D").Replace(phoneNumber, string.Empty);
            return Convert.ToInt64(phoneNumber.TrimStart('8')).ToString("+7(###)###-##-##");
        }
        return phoneNumber;
    }

    public IActionResult LastAppeals(string PhoneNumber)
    {
        string a = FormatPhoneNumber(PhoneNumber); 
        ViewBag.LastAppeals = _repository.DataAppeal.Include(i => i.SprStatus).Where(w => w.PhoneNumber == a || w.PhoneNumber == PhoneNumber).OrderByDescending(o => o.DateAdd).Take(5);
        ViewBag.LastCalls = _repository.DataAppealCall.Where(w => w.PhoneNumber == PhoneNumber || w.PhoneNumber == a).OrderByDescending(o => o.DateCall).Take(4).ToArray();

        return PartialView("Modal/LastAppeals");
    }

    /// <summary>
    /// Добавление нового обращения
    /// </summary>
    /// <param name="DataAppeal">объект</param>
    /// <returns>перенаправляет на страницу обращений</returns>
    [HttpPost]
    public IActionResult SubmitNewAppealSave(DataAppeal dataAppeal)
    {
        if (dataAppeal.Id == Guid.Empty)
        {
            Random rand = new();
            var employee = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);
            dataAppeal.NumberAppeal = string.Format("{0}{3}{1}{2}", "APEL", DateTime.Now.ToString("ddMMyyHHmmssffff"), rand.Next(1, 5), rand.Next(0, 9));
            dataAppeal.DateAdd = DateTime.Now;
            dataAppeal.SprEmployeesId = employee?.Id ?? Guid.Empty;
            dataAppeal.EmployeesNameAdd = employee.EmployeesName;
            dataAppeal.SprStatusId = 0;
            dataAppeal.SprRoutesStageIdCurrent = 1;
            //_repository.Insert(dataAppeal);                    
        }
        DataAppeal model = new()
        {
            NumberAppeal = dataAppeal.NumberAppeal,
            PhoneNumber = dataAppeal.PhoneNumber
        };
        return PartialView("Modal/NewAppealResult", model);
    }

    public IActionResult Questions()
    {
        List<SprQuestion> model = _repository.SprQuestion.ToList();
        return PartialView("Modal/Questions", model);
    }

    public IActionResult SaveQuestions(List<Guid> questions_id, string PhoneNumber)
    {
        var employee = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);
        if (questions_id != null)
        {
            foreach (var item in questions_id)
            {
                DataQuestion model = new()
                {
                    DateQuestion = DateTime.Now,
                    PhoneNumber = PhoneNumber,
                    SprEmployeesId = employee?.Id ?? Guid.Empty,
                    EmployeesNameAdd = employee.EmployeesName,
                    SprQuestionId = item
                };
                _repository.Insert(model);
            }
        }
        return Json(questions_id);
    }

    public IActionResult Survey()
    {
        List<SprSurveyQuestion> model = _repository.SprSurveyQuestion.Include(i => i.SprSurveyAnswer).Where(w => w.IsRemove != true).ToList();
        return PartialView("Modal/SurveyQuestion", model);
    }
    public IActionResult SaveSurveyAnswers(List<Guid> survey_answer_id, string PhoneNumber)
    {
        var employee = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);
        if (survey_answer_id != null)
        {
            foreach (var item in survey_answer_id)
            {
                var answer = _repository.SprSurveyAnswer.Include(i => i.SprSurveyQuestion).Where(w => w.Id == item).FirstOrDefault();
                DataSurvey model = new()
                {
                    Answer = answer.Answer,
                    Question = answer.SprSurveyQuestion.Question,
                    DateSurvey = DateTime.Now,
                    PhoneNumber = PhoneNumber,
                    SprEmployeesId = employee?.Id ?? Guid.Empty,
                    EmployeesNameAdd = employee.EmployeesName,
                    SprSurveyAnswerId = item,
                    SprSurveyQuestionId = answer.SprSurveyQuestion.Id,
                };
                _repository.Insert(model);
            }
        }
        return Json(survey_answer_id);
    }

    public IActionResult CallCommentts(string NumberAppeal)
    {
        AppealViewModel model = new();
        if (NumberAppeal != null && NumberAppeal != "")
        {
            Guid appealId = _repository.DataAppeal.Where(w => w.NumberAppeal == NumberAppeal).FirstOrDefault().Id;
            var employee = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);
            model = new AppealViewModel
            {
                DataAppealCommenttList = _repository.DataAppealCommentt.Include(i => i.DataAppealCommenttRecipients).Where(dsc => dsc.DataAppealId == appealId).OrderBy(o => o.DateAdd).ToList(),
                DataAppealCommenttRecipientList = _repository.DataAppealCommenttRecipients.Include(i => i.SprEmployees).Where(w => w.DataAppealCommentt.DataAppealId == appealId).ToList(),
                DataAppealId = appealId,
                EmployeeId = employee.Id,
                CurrentEmployeeId = _repository.DataAppeal.Where(w => w.Id == appealId).FirstOrDefault()?.SprEmployeesIdCurrent ?? Guid.Empty
            };
            ViewBag.AppealNumber = _repository.DataAppeal.SingleOrDefault(ds => ds.Id == appealId)?.NumberAppeal;
        }
        return PartialView("Modal/Commentts", model);
    }

}