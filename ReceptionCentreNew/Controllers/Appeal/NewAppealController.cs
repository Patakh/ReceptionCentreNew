using ReceptionCentreNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReceptionCentreNew.Data.Context.App;
using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Areas.Identity.User;
using ReceptionCentreNew.Data.Context.App.Abstract;

namespace ReceptionCentreNew.Controllers.Appeal;
public partial class AppealController : Controller
{
    public int PageSize = 7;

    #region Инициализация Repository
    private IRepository _repository;
    private string? UserName;
    public AppealController(IRepository repo, UserManager<ApplicationUser> userManager)
    {
        _repository = repo;
        UserName = _repository.SprEmployees.First(s => s.Id == userManager.GetUserAsync(User).Result.EmployeeId.Value).EmployeesName;
    }
    #endregion
    // GET: NewApeeal
    public IActionResult NewAppeal(Guid? emailId, Guid? callId, string ApplicantName)
    {
        ViewBag.SprCategory = new SelectList(_repository.SprCategory, "Id", "CategoryName");
        ViewBag.SprType = new SelectList(_repository.SprType, "Id", "TypeName");
        ViewBag.SprTypeDifficulty = new SelectList(_repository.SprTypeDifficulty.OrderBy(o => o.CountDay), "Id", "TypeName");
        ViewBag.SprSubjectTreatment = new SelectList(_repository.SprSubjectTreatment, "Id", "SubjectName");
        ViewBag.SprMfc = new SelectList(_repository.SprMfc.OrderBy(o => o.MfcName), "Id", "MfcName");
        ViewBag.SprTransferEmployees = new SelectList(_repository.SprEmployees.Where(w => w.IsRemove != true && w.CanTakeAppeal == true).Select(s => new { s.Id, s.EmployeesName, s.SprEmployeesJobPos.JobPosName }), "Id", "EmployeesName", "JobPosName", _repository.SprEmployees.Where(w => w.EmployeesLogin == User.Identity.Name).FirstOrDefault().Id.ToString());
        ViewBag.RouteStages = new SelectList(_repository.SprRoutesStage.Where(w => w.Id != 1).OrderBy(o => o.Id), "Id", "stage_name");

        ViewBag.EmailId = emailId;
        ViewBag.CallId = callId;
        DataAppeal model = new DataAppeal()
        {
            PhoneNumber = callId != null ? _repository.DataAppealCall.Where(w => w.Id == callId).SingleOrDefault()?.PhoneNumber : "",
            ApplicantName = ApplicantName,
        };
        return PartialView("NewAppeal/NewAppeal", model);
    }
    public string FormatPhoneNumber(string phoneNumber)
    {
        string originalValue = phoneNumber;
        phoneNumber = new System.Text.RegularExpressions.Regex(@"\D").Replace(phoneNumber, string.Empty);
        string value = originalValue.TrimStart('8');
        if (phoneNumber.Length == 7)
            return Convert.ToInt64(value).ToString("###-####");
        if (phoneNumber.Length == 9)
            return Convert.ToInt64(value).ToString("###-###-####");
        if (phoneNumber.Length == 10)
            return Convert.ToInt64(value).ToString("###-###-####");
        if (phoneNumber.Length > 10)
            return Convert.ToInt64(value).ToString("+7(###)###-##-##");

        return phoneNumber;
    }
    public IActionResult NewAppealEmail(Guid? emailId)
    {
        DataAppealEmail email = _repository.DataAppealEmail.Where(w => w.Id == emailId).FirstOrDefault();
        return PartialView("NewAppeal/_PartialEmail", email);
    }
    public IActionResult NewAppealCall(Guid? callId)
    {
        DataAppealCall call = _repository.DataAppealCall.Where(w => w.Id == callId).FirstOrDefault();
        return PartialView("NewAppeal/_PartialCall", call);
    }

    /// <summary>
    /// Добавление нового обращения
    /// </summary>
    /// <param name="DataAppeal">объект</param>
    /// <returns>перенаправляет на страницу обращений</returns>
    [HttpPost]
    public IActionResult SubmitNewAppealSave(DataAppeal dataAppeal, Guid? emailId, Guid? smsId, Guid? callId, IFormFile uploadFile, Guid? transferEmployeeId, int? routeStageId, bool modal = false)
    {
        if (ModelState.IsValid)
        {
            if (dataAppeal.Id == Guid.Empty)
            {
                string ShortName = _repository.SprCategory.Where(w => w.Id == dataAppeal.SprCategoryId).FirstOrDefault().ShortName;
                Random rand = new();
                var employee = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == User.Identity.Name);
                dataAppeal.NumberAppeal = String.Format("{0}{3}{1}{2}", ShortName, DateTime.Now.ToString("ddMMyyHHmmssffff"), rand.Next(1, 5), rand.Next(0, 9));
                dataAppeal.DateAdd = DateTime.Now;
                dataAppeal.SprEmployeesId = employee?.Id ?? Guid.Empty;
                dataAppeal.EmployeesNameAdd = employee.EmployeesName;
                dataAppeal.SprStatusId = 0;
                dataAppeal.SprRoutesStageIdCurrent = 1;
                _repository.Insert(dataAppeal);
                //===Email===
                if (emailId.HasValue)
                {
                    DataAppealEmail email = _repository.DataAppealEmail.Where(w => w.Id == emailId).SingleOrDefault();
                    email.DataAppealId = dataAppeal.Id;
                    _repository.Update(email);
                }
                //===Sms===
                if (smsId.HasValue)
                {
                    DataAppealMessage sms = _repository.DataAppealMessage.Where(w => w.Id == smsId).SingleOrDefault();
                    sms.DataAppealId = dataAppeal.Id;
                    _repository.Update(sms);
                }
                //===Call===
                if (callId.HasValue)
                {
                    DataAppealCall call = _repository.DataAppealCall.Where(w => w.Id == callId).SingleOrDefault();
                    call.DataAppealId = dataAppeal.Id;
                    _repository.Update(call);
                }
                //===File===
                if (uploadFile != null)
                {
                    string theFileName = Path.GetFileNameWithoutExtension(uploadFile.FileName);
                    byte[] theFilebytes = new byte[uploadFile.Length];
                    using (MemoryStream memoryStream = new())
                    {
                        uploadFile.CopyToAsync(memoryStream);
                        theFilebytes = memoryStream.ToArray();
                    }
                    DataAppealFile saveFileModel = new() // Модель для сохранения в таблицу 
                    {
                        DataAppealId = dataAppeal.Id,
                        FileName = theFileName,
                        FileExt = Path.GetExtension(uploadFile.FileName),
                        FileSize = theFilebytes.Length,
                        EmployeesNameAdd = employee.EmployeesName,
                        SprEmployeesId = employee?.Id ?? Guid.Empty,
                        DateAdd = DateTime.Now
                    };
                    _repository.Insert(saveFileModel); //Запись в таблицу 

                    var settings = _repository.SprSetting.ToList();
                    var ftpModel =
                        new
                        {
                            ftpServer = settings.SingleOrDefault(ss => ss.ParamName == "ftp_server")?.ParamValue,
                            ftpFolder = settings.SingleOrDefault(ss => ss.ParamName == "ftp_folder_files")?.ParamValue,
                            ftpLogin = settings.SingleOrDefault(ss => ss.ParamName == "ftp_user")?.ParamValue,
                            ftpPass = CRPassword.Encrypt(settings.SingleOrDefault(ss => ss.ParamName == "ftp_password")?.ParamValue)
                        };

                    FtpFileModel ftp = new();
                    var res = ftp.UploadFileFtp(theFilebytes, ftpModel.ftpServer, ftpModel.ftpLogin, ftpModel.ftpPass, saveFileModel.Id + Path.GetExtension(uploadFile.FileName));
                }
                //===Route stage===
                if (routeStageId.HasValue)
                {
                    DataAppealRoutesStage route_stage = new()
                    {
                        DateStart = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day),
                        TimeStart = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond),
                        DataAppealId = dataAppeal.Id,
                        SprEmployeesId = transferEmployeeId.HasValue ? (Guid)transferEmployeeId : employee?.Id ?? Guid.Empty,
                        SprRoutesStageId = (int)routeStageId,
                        EmployeesNameAdd = employee?.EmployeesName ?? string.Empty
                    };
                    _repository.Insert(route_stage);
                    //===уведомление о передаче дела
                    if (route_stage.SprRoutesStageId != 1 && employee.Id != route_stage.SprEmployeesId)
                    {
                        SignalRAlerts(employee.Id, "Вам передано обращение #" + dataAppeal.NumberAppeal);
                    }
                }
                /*return RedirectToAction("AppealInfo", "Appeal", new { number = dataAppeal.NumberAppeal, modal = true });*//* Json(dataAppeal.NumberAppeal);*/
            }
            else
            {
                DataAppeal model = _repository.DataAppeal.Where(w => w.Id == dataAppeal.Id).SingleOrDefault();
                model.ApplicantName = dataAppeal.ApplicantName;
                model.Email = dataAppeal.Email;
                model.PhoneNumber = dataAppeal.PhoneNumber;
                model.TextAppeal = dataAppeal.TextAppeal;

                model.SprCategoryId = dataAppeal.SprCategoryId;
                model.SprSubjectTreatmentId = dataAppeal.SprSubjectTreatmentId;
                model.SprTypeDifficultyId = dataAppeal.SprTypeDifficultyId;
                model.SprTypeId = dataAppeal.SprTypeId;
                model.SprMfcId = dataAppeal.SprMfcId;
                model.CaseNumber = dataAppeal.CaseNumber;

                model.EmployeesNameModify = UserName;
                model.DateModify = DateTime.Now;
                _repository.Update(model);

                if (modal)
                { return RedirectToAction("AppealInfo", new { number = model.NumberAppeal, modal = true }); }
                else
                {
                    return RedirectToAction("AppealInfo", new { number = model.NumberAppeal });
                }
            }
            return RedirectToAction("AppealInfo", new { number = dataAppeal.NumberAppeal, modal = true });
            //return Json(dataAppeal.NumberAppeal);
        }
        throw new Exception("Ошибка валидации!");
    }


    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Email  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

    /// <summary>
    /// Для прикрепления почтового сообщения к новому обращению
    /// </summary>
    /// <returns>частичное представление модального окна</returns>
    [HttpPost]
    public IActionResult newPartialModalAttachEmail(string search)
    {
        var noAttachEmails = _repository.DataAppealEmail.Where(w => w.DataAppealId == null);
        noAttachEmails = string.IsNullOrEmpty(search) ? noAttachEmails :
            search.ToLower().Split().Aggregate(noAttachEmails, (current, item) => current.Where(h => h.Email.ToLower().Contains(item) || h.TextEmail.ToLower().Contains(item) || h.Caption.ToLower().Contains(item) || h.EmployeesNameAdd.ToLower().Contains(item))).OrderByDescending(o => o.DateEmail);

        ViewBag.SearchNewAttach = search;
        ViewBag.newEmailCount = noAttachEmails.Count();
        return PartialView("NewAppeal/newPartialAttachEmail", noAttachEmails);
    }
    #endregion

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  SMS  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

    /// <summary>
    /// Для прикрепления сообщения к новому обращению
    /// </summary>
    /// <returns>частичное представление модального окна</returns>
    [HttpPost]
    public IActionResult newPartialAttachSms(string search)
    {
        var noAttachSms = _repository.DataAppealMessage.Where(w => w.DataAppealId == null);
        noAttachSms = string.IsNullOrEmpty(search) ? noAttachSms :
            search.ToLower().Split().Aggregate(noAttachSms, (current, item) => current.Where(h => h.PhoneNumber.ToLower().Contains(item) || h.TextMessage.ToLower().Contains(item) || h.DateMessage.ToShortDateString().Contains(item) || h.EmployeesNameAdd.ToLower().Contains(item))).OrderByDescending(o => o.DateMessage);

        ViewBag.SearchNewAttachSms = search;
        ViewBag.newSmsCount = noAttachSms.Count();
        return PartialView("NewAppeal/newPartialAttachSms", noAttachSms);
    }
    #endregion

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Call  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

    /// <summary>
    /// Для прикрепления звонка к новому обращению
    /// </summary>
    /// <returns>частичное представление модального окна</returns>
    [HttpPost]
    public IActionResult newPartialModalAttachCall(string search)
    {
        DateTime dat = new();
        dat = DateTime.Now.AddDays(-7);
        var noAttachCalls = _repository.DataAppealCall.Where(w => w.DataAppealId == null && w.DateCall >= dat);
        noAttachCalls = string.IsNullOrEmpty(search) ? noAttachCalls :
            search.ToLower().Split().Aggregate(noAttachCalls, (current, item) => current.Where(h => h.PhoneNumber.ToLower().Contains(item) || h.DateCall.ToString().Contains(item) || h.EmployeesNameAdd.ToLower().Contains(item)));

        ViewBag.SearchNewAttachCall = search;
        ViewBag.newCallCount = noAttachCalls.Count();
        return PartialView("NewAppeal/newPartialAttachCall", noAttachCalls.OrderByDescending(o => o.DateCall));
    }
    #endregion

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Text Appeal Template  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

    [HttpPost]
    public IActionResult newPartialModalAddTextAppealTemplate()
    {
        return PartialView("NewAppeal/newPartialModalAddTextAppealTemplates", new SprEmployeesTextAppealTemplate() { EmployeesNameAdd = UserName });
    }
    [HttpPost]
    public IActionResult newPartialModalAppendTextAppealTemplate()
    {
        IEnumerable<SprEmployeesTextAppealTemplate> model = _repository.SprEmployeesTextAppealTemplate.Where(w => w.IsRemove != true);
        return PartialView("NewAppeal/newPartialModalAppendTextAppealTemplates", model);
    }

    [HttpPost]
    public IActionResult SubmitNewModalTextAppealTemplateSave(SprEmployeesTextAppealTemplate t)
    {
        var employee = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == User.Identity.Name);
        t.SprEmployeesId = employee?.Id ?? Guid.Empty;
        if (ModelState.IsValid)
        {
            if (t.Id == Guid.Empty)
            {
                t.DateAdd = DateTime.Now;
                _repository.Insert(t);
            }
            else
            {
                t.EmployeesNameModify = UserName;
                t.DateModify = DateTime.Now;
                _repository.Update(t);
            }
            return null;
        }
        throw new Exception("Ошибка валидации!");
    }

    [HttpPost]
    public IActionResult SubmitNewTextAppealTemplateDelete(Guid templateId)
    {
        SprEmployeesTextAppealTemplate deleteT = _repository.SprEmployeesTextAppealTemplate.SingleOrDefault(so => so.Id == templateId);
        deleteT.IsRemove = true;
        _repository.Delete(deleteT);
        IEnumerable<SprEmployeesTextAppealTemplate> model = _repository.SprEmployeesTextAppealTemplate.Where(w => w.IsRemove != true);
        return PartialView("NewAppeal/newPartialModalAppendTextAppealTemplates", model);
    }
    #endregion
}