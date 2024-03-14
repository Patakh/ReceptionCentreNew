using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Filters;
using ReceptionCentreNew.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using ReceptionCentreNew.Hubs;

namespace ReceptionCentreNew.Controllers.Appeal;
[ClientErrorHandler]
[Authorize(Roles = "superadmin, admin, operator, expert")]
public partial class AppealController : Controller
{
    // GET: Appeal
    public IActionResult Appeals()
    {
        var employees = _repository.SprEmployees.Where(e => e.IsRemove != true).OrderBy(o => o.EmployeesName);
        if (!User.IsInRole("superadmin") && !User.IsInRole("admin") && !User.IsInRole("operator"))
        {
            employees = employees.Where(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name).OrderBy(o => o.EmployeesName);
        }
        Guid empId = employees.Where(w => w.EmployeesLogin == SignInManager.Context.User.Identity.Name).FirstOrDefault().Id;

        ViewBag.SprEmployeesId = empId;
        ViewBag.SprEmployees = new SelectList(employees, "Id", "EmployeesName", empId);
        ViewBag.SprCategory = new SelectList(_repository.SprCategory.Where(e => e.IsRemove != true), "Id", "CategoryName");
        ViewBag.SprType = new SelectList(_repository.SprType, "Id", "TypeName");
        ViewBag.SprTypeDifficulty = new SelectList(_repository.SprTypeDifficulty.Where(e => e.IsRemove != true), "Id", "TypeName");
        ViewBag.SprSubject = new SelectList(_repository.SprSubjectTreatment.Where(e => e.IsRemove != true), "Id", "SubjectName");
        ViewBag.SprStatus = new SelectList(_repository.SprStatus, "Id", "StatusName", 0);
        return View();
    }

    public IActionResult AppealInfo(string number, Guid? notificationId, bool modal = false)
    {
        if (number != string.Empty && number != "" && number != null)
        {
            if (notificationId.HasValue)
            {
                DataEmployeesNotification notify = _repository.DataEmployeesNotification.Where(w => w.Id == notificationId).FirstOrDefault();
                if (notify.IsActive == true)
                {
                    notify.IsActive = false;
                    _repository.Update(notify);
                }
            }
            DataAppealSelect dataAppeal = _repository.FuncDataAppealInfo(number);
            ViewBag.CallCount = _repository.DataAppealCall.Where(w => w.DataAppealId == dataAppeal.OutDataAppealId).Count();
            ViewBag.EmailCount = _repository.DataAppealEmail.Where(w => w.DataAppealId == dataAppeal.OutDataAppealId).Count();
            ViewBag.FileCount = _repository.DataAppealFile.Where(w => w.DataAppealId == dataAppeal.OutDataAppealId && w.IsRemove != true).Count();
            ViewBag.RouteStageCount = _repository.DataAppealRoutesStage.Where(w => w.DataAppealId == dataAppeal.OutDataAppealId).Count();
            ViewBag.CommentsCount = _repository.DataAppealCommentt.Where(w => w.DataAppealId == dataAppeal.OutDataAppealId).Count();
            ViewBag.NotithCount = _repository.DataAppealCall.Where(w => w.DataAppealId == dataAppeal.OutDataAppealId).Count() + _repository.DataAppealMessage.Where(w => w.DataAppealId == dataAppeal.OutDataAppealId).Count();
            ViewBag.ChangeLogCount = _repository.DataChangeLog.Where(w => w.RowId == dataAppeal.OutDataAppealId).Count();

            var employees = _repository.SprEmployees.Where(e => e.IsRemove != true);
            ViewBag.SprEmployeesId = employees.Where(w => w.EmployeesLogin == SignInManager.Context.User.Identity.Name).FirstOrDefault().Id;

            if (modal)
            {
                return PartialView("AppealInfoModal", dataAppeal);
            }
            else
            {
                return View(dataAppeal);
            }
        }
        else
        {
            return RedirectToAction("Appeals");
        }
    }

    public IActionResult PartialTableAppeals(Guid? SprEmployeeId, short? period, Guid? SprTypeId, Guid? SprTypeDifficultyId, Guid? SprSubjectId, int? SprStatusId, Guid? SprCategoryId, string search, int page = 1)
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
        var appeals = _repository.FuncDataAppealSelect(SprEmployeeId, dateStart, DateTime.Now.AddDays(1), SprTypeId, SprTypeDifficultyId, SprCategoryId, SprSubjectId, SprStatusId);
        appeals = String.IsNullOrEmpty(search) ? appeals :
            search.ToLower().Split().Aggregate(appeals, (current, item) => current.Where(h => h.OutApplicantName.ToLower().Contains(item)
            || (h.OutPhoneNumber != null ? h.OutPhoneNumber.ToLower().Contains(item) : false)
            || (h.OutNumberAppeal != null ? h.OutNumberAppeal.ToLower().Contains(item) : false)
            || (h.OutMfcName != null ? h.OutMfcName.ToLower().Contains(item) : false)
            || (h.OutEmployeesName != null ? h.OutEmployeesName.ToLower().Contains(item) : false)
            || (h.OutDateAdd != null ? h.OutDateAdd.ToString().ToLower().Contains(item) : false)
            || (h.OutStatusName != null ? h.OutStatusName.ToLower().Contains(item) : false)
            || (h.OutStageNameCurrent != null ? h.OutStageNameCurrent.ToLower().Contains(item) : false)));

        AppealViewModel model = new AppealViewModel
        {
            DataAppealSelectList = appeals.OrderByDescending(o => o.OutDateAdd).Skip((page - 1) * PageSize).Take(PageSize),
            PageInfo = new PageInfo
            {
                MaxPageList = 5,
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = appeals.Count()
            },
        };
        ViewBag.Search = search;
        ViewBag.Period = period;
        ViewBag.SprEmployeesId = SprEmployeeId;
        ViewBag.SprCategoryId = SprCategoryId;
        ViewBag.SprTypeId = SprTypeId;
        ViewBag.SprTypeDifficultyId = SprTypeDifficultyId;
        ViewBag.SprSubjectId = SprSubjectId;
        ViewBag.SprStatusId = SprStatusId;
        return PartialView(model);
    }

    public IActionResult EditAppeal(Guid appealId, bool modal = false)
    {
        DataAppeal model = _repository.DataAppeal.Where(w => w.Id == appealId).SingleOrDefault();
        ViewBag.SprCategory = new SelectList(_repository.SprCategory.OrderBy(o => o.CategoryName), "Id", "CategoryName", model.SprCategoryId);
        ViewBag.SprType = new SelectList(_repository.SprType.OrderBy(o => o.TypeName), "Id", "TypeName", model.SprTypeId);
        ViewBag.SprTypeDifficulty = new SelectList(_repository.SprTypeDifficulty.OrderBy(o => o.TypeName), "Id", "TypeName", model.SprTypeDifficultyId);
        ViewBag.SprSubjectTreatment = new SelectList(_repository.SprSubjectTreatment.OrderBy(o => o.SubjectName), "Id", "SubjectName", model.SprSubjectTreatmentId);
        ViewBag.SprMfc = new SelectList(_repository.SprMfc.OrderBy(o => o.MfcName), "Id", "MfcName", model.SprMfcId);

        if (modal)
            return PartialView("EditAppealModal", model);
        else
            return PartialView("EditAppeal", model);
    }

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Этапы  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

    /// <summary>
    /// Следующий этап
    /// </summary>
    /// <returns>частичное представление модального окна</returns>
    [HttpPost]
    public IActionResult PartialModalNextRouteStage(Guid appealId)
    {
        var routeStages = _repository.FuncDataAppealRouteStageNext(appealId);
        ViewBag.AppealId = appealId;
        ViewBag.Employees = new SelectList(_repository.SprEmployees.Where(w => w.CanTakeAppeal == true).Select(s => new { s.Id, s.EmployeesName, s.SprEmployeesJobPos.JobPosName }), "Id", "EmployeesName", "JobPosName", _repository.SprEmployees.Where(w => w.EmployeesLogin == SignInManager.Context.User.Identity.Name).FirstOrDefault()?.Id.ToString());
        return PartialView("AppealDetails/RouteStages/PartialModalNextRouteStage", routeStages);
    }

    /// <summary>
    /// добавляет этап
    /// </summary>
    /// <returns>частичное представление таблицы</returns>
    [HttpPost]
    public IActionResult SubmitNextRouteStageSave(Guid? employeeId, int routeStageId, Guid appealId, string modal_routes_stage_commentt)
    {
        var employee = employeeId != null ? _repository.SprEmployees.SingleOrDefault(se => se.Id == employeeId) : _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);
        var infoId = _repository.DataAppeal.SingleOrDefault(ds => ds.Id == appealId).NumberAppeal;
        DataAppealRoutesStage route_stages_last = _repository.DataAppealRoutesStage.Where(w => w.DataAppealId == appealId).OrderByDescending(o => o.DateStart).ThenByDescending(o => o.TimeStart).FirstOrDefault();

        DataAppealRoutesStage model = new()
        {
            DateStart = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            TimeStart = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
            DataAppealId = appealId,
            SprEmployeesId = employee.Id,
            SprRoutesStageId = routeStageId,
            EmployeesNameAdd = UserName
        };
        _repository.Insert(model);

        if (route_stages_last.SprRoutesStageId != 1 && employee.Id != route_stages_last.SprEmployeesId)
            SignalRAlerts(employee.Id, "Вам передано обращение #" + _repository.DataAppeal.Where(w => w.Id == appealId).FirstOrDefault().NumberAppeal);

        if (modal_routes_stage_commentt != "" && modal_routes_stage_commentt != null)
        {
            var emp = _repository.SprEmployees.Where(w => w.EmployeesLogin == SignInManager.Context.User.Identity.Name).SingleOrDefault();
            DataAppealCommentt route_stage = new()
            {
                Commentt = modal_routes_stage_commentt,
                DataAppealId = appealId,
                DateAdd = DateTime.Now,
                SprEmployeesId = emp.Id,
                EmployeesNameAdd = emp.EmployeesName
            };
            _repository.Insert(route_stage);
        }
        return RedirectToAction("PartialTableRouteStages", new { appealId });
    }

    public IActionResult PartialTableRouteStages(Guid appealId)
    {
        var employeeId = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name)?.Id ?? Guid.Empty;
        AppealViewModel model = new()
        {
            DataAppealId = appealId,
            DataAppealRoutesStageList = _repository.FuncDataAppealRouteStageSelect(appealId).OrderBy(o => o.OutDateStart).ThenBy(t => t.OutTimeStart),
            CurrentEmployeeId = _repository.DataAppeal.Where(w => w.Id == appealId).FirstOrDefault()?.SprEmployeesIdCurrent ?? Guid.Empty,
            EmployeeId = employeeId
        };

        var routeStages = _repository.FuncDataAppealRouteStageNext(appealId);
        ViewBag.Employees = new SelectList(_repository.SprEmployees.Where(w => w.IsRemove != true && w.CanTakeAppeal == true).Select(s => new { s.Id, s.EmployeesName, s.SprEmployeesJobPos.JobPosName }), "Id", "EmployeesName", "JobPosName", _repository.SprEmployees.Where(w => w.EmployeesLogin == SignInManager.Context.User.Identity.Name).FirstOrDefault()?.Id.ToString());
        ViewBag.RouteStages = new SelectList(routeStages, "OutSprRoutesStageId", "OutStageName");
        return PartialView("AppealDetails/RouteStages/PartialTableRouteStages", model);
    }

    #endregion

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Примечания  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|
    public IActionResult PartialTableCommentts(Guid appealId)
    {
        var employee = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);
        AppealViewModel model = new()
        {
            DataAppealCommenttList = _repository.DataAppealCommentt.Include(i => i.DataAppealCommenttRecipients).Where(dsc => dsc.DataAppealId == appealId).OrderBy(o => o.DateAdd).ToList(),
            DataAppealCommenttRecipientList = _repository.DataAppealCommenttRecipients.Include(i => i.SprEmployees).Where(w => w.DataAppealCommentt.DataAppealId == appealId).ToList(),
            DataAppealId = appealId,
            EmployeeId = employee.Id,
            CurrentEmployeeId = _repository.DataAppeal.Where(w => w.Id == appealId).FirstOrDefault()?.SprEmployeesIdCurrent ?? Guid.Empty
        };
        ViewBag.AppealNumber = _repository.DataAppeal.SingleOrDefault(ds => ds.Id == appealId)?.NumberAppeal;
        // если не текущий заявитель, не админ и суперадмин и есть роль
        ViewBag.SprEmployees = new SelectList(_repository.SprEmployees
                     .Where(w => w.IsRemove != true && w.Id != employee.Id && w.SprEmployeesRoleJoin.FirstOrDefault().SprEmployeesRoleId != 1 && w.SprEmployeesRoleJoin.FirstOrDefault() != null)
                     .OrderBy(o => o.SprEmployeesJobPos.JobPosName)
                     .Select(s => new { s.Id, s.EmployeesName, s.SprEmployeesJobPos.JobPosName }), "Id", "EmployeesName", _repository.SprEmployees.Where(w => w.EmployeesLogin == SignInManager.Context.User.Identity.Name).FirstOrDefault().Id);
        ViewBag.SprEmployeesMessageTemplates = new SelectList(_repository.SprEmployeesMessageTemplate.Where(w => w.IsRemove != true && w.SprEmployeesId == employee.Id).OrderByDescending(o => o.Sort), "Id", "MessageText");
        return PartialView("AppealDetails/Comments/PartialTableCommentsView", model);
    }

    public IActionResult SubmitCommentSave(DataAppealCommentt dataAppealCommentt, Guid[] recipients)
    {
        var employee = _repository.SprEmployees.Include(i => i.SprEmployeesJobPos).Include(ii => ii.SprEmployeesDepartment).SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);

        if (dataAppealCommentt.Id == Guid.Empty)
        {
            dataAppealCommentt.EmployeesNameAdd = employee?.EmployeesName ?? "";
            dataAppealCommentt.SprEmployeesId = employee?.Id ?? Guid.Empty;
            dataAppealCommentt.DateAdd = DateTime.Now;
            _repository.Insert(dataAppealCommentt);
            if (recipients != null)
            {
                foreach (var item in recipients)
                {
                    DataAppealCommenttRecipients model = new()
                    {
                        DataAppealCommenttId = dataAppealCommentt.Id,
                        SprEmployeesId = item,
                    };
                    _repository.Insert(model);

                    DataEmployeesNotification notif = new()
                    {
                        DataAppealId = dataAppealCommentt.DataAppealId,
                        SprNotificationId = 2,
                        SprEmployeesId = item,
                        DateReceipt = DateTime.Now,
                        IsActive = true
                    };
                    _repository.Insert(notif);
                    SignalRAlerts(item, "Добавлено новое примечание " + "в деле #" + _repository.DataAppeal.Where(w => w.Id == dataAppealCommentt.DataAppealId).FirstOrDefault().NumberAppeal);
                }
            }
        }
        else
        {
            var editComment = _repository.DataAppealCommentt.Where(w => w.Id == dataAppealCommentt.Id).SingleOrDefault();
            editComment.EmployeesNameModify = UserName;
            editComment.DateModify = DateTime.Now;
            editComment.Commentt = dataAppealCommentt.Commentt;
            _repository.Update(editComment);
        }
        return RedirectToAction("PartialTableCommentts", new { appealId = dataAppealCommentt.DataAppealId });

    }
      
    /// <summary>
    /// Удаляет запись по указанному Id
    /// </summary>
    /// <param name="commentId">Id комментария</param>
    /// <returns>частичное представление таблицы </returns>
    [HttpPost]
    public IActionResult SubmitCommentDelete(Guid commentId)
    {
        DataAppealCommentt deletedComment = _repository.DataAppealCommentt.SingleOrDefault(w => w.Id == commentId);
        deletedComment.IsRemove = true;
        deletedComment.EmployeesNameModify = "Admin";
        _repository.Update(deletedComment);
        return RedirectToAction("PartialTableCommentts", new { appealId = deletedComment.DataAppealId });
    }

    /// <summary>
    /// Восстанавливает запись по указанному Id
    /// </summary>
    /// <param name="commentId">Id</param>
    /// <returns>частичное представление таблицы</returns>
    [HttpPost]
    public IActionResult SubmitCommentRecovery(Guid commentId)
    {
        DataAppealCommentt recoveryComment = _repository.DataAppealCommentt.SingleOrDefault(w => w.Id == commentId);
        recoveryComment.IsRemove = false;
        _repository.Update(recoveryComment);
        return RedirectToAction("PartialTableCommentts", new { appealId = recoveryComment.DataAppealId });
    }
    #endregion

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Шаблоны сообщений  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

    /// <summary>
    /// Добавление
    /// </summary>
    /// <returns>частичное представление модального окна</returns> 
    public IActionResult PartialModalAddEmployeesMessageTemplate(string number)
    {
        ViewBag.AppealNumber = number;
        return PartialView("AppealDetails/Comments/MessageTemplates/PartialModalAddMessageTemplate", new SprEmployeesMessageTemplate { EmployeesNameAdd = UserName });
    }

    /// <summary>
    /// Изменение
    /// </summary>
    /// <returns>частичное представление модального окна</returns>
    [HttpPost]
    public IActionResult PartialModalEditEmployeesMessageTemplate(Guid templateId, string number)
    {
        ViewBag.AppealNumber = number;
        return PartialView("AppealDetails/Comments/MessageTemplates/PartialModalEditMessageTemplate", _repository.SprEmployeesMessageTemplate.SingleOrDefault(ed => ed.Id == templateId));
    }

    /// <summary>
    /// Сохраняет изменения или добавляет
    /// </summary>
    /// <param name="template">объект</param>
    /// <returns>частичное представление таблицы</returns>
    [HttpPost]
    public IActionResult SubmitEmployeesMessageTemplateSave(SprEmployeesMessageTemplate template, string number)
    {
        var employee = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);
        if (template.Id == Guid.Empty)
        {
            template.SprEmployeesId = employee.Id;
            template.EmployeesNameAdd = UserName;
            template.DateAdd = DateTime.Now;
            _repository.Insert(template);
        }
        else
        {
            template.SprEmployeesId = employee.Id;
            template.EmployeesNameModify = UserName;
            template.DateModify = DateTime.Now;
            _repository.Update(template);
        }
        return RedirectToAction("PartialTableEmployeesMessageTemplate", new { number = number });
    }

    /// <summary>
    /// Восстанавливает запись по указанному Id
    /// </summary>
    /// <param name="Id">Id</param>
    /// <returns>частичное представление таблицы</returns>
    [HttpPost]
    public IActionResult SubmiEmployeesMessageTemplateRecovery(Guid templateId, string number)
    {
        SprEmployeesMessageTemplate recoveryTemplate = _repository.SprEmployeesMessageTemplate.SingleOrDefault(so => so.Id == templateId);
        recoveryTemplate.IsRemove = false;
        _repository.Update(recoveryTemplate);
        return RedirectToAction("PartialTableEmployeesMessageTemplate", new { number = number });
    }

    /// <summary>
    /// Удаляет запись по указанному Id
    /// </summary>
    /// <param name="Id">Id</param>
    /// <returns>частичное представление таблицы</returns>
    [HttpPost]
    public IActionResult SubmitEmployeesMessageTemplateDelete(Guid templateId, string number)
    {
        SprEmployeesMessageTemplate deleteTemplate = _repository.SprEmployeesMessageTemplate.SingleOrDefault(sed => sed.Id == templateId);

        deleteTemplate.IsRemove = true;
        deleteTemplate.EmployeesNameModify = UserName;
        deleteTemplate.DateModify = DateTime.Now;
        _repository.Update(deleteTemplate);
        return RedirectToAction("PartialTableEmployeesMessageTemplate", new { number = number });
    }

    public IActionResult PartialTableEmployeesMessageTemplate(string number, bool isRemove = false, int page = 1)
    {
        ViewBag.IsRemove = isRemove;
        ViewBag.AppealNumber = number;
        var employee = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);
        var templates = _repository.SprEmployeesMessageTemplate.Where(w => w.SprEmployeesId == employee.Id);
        templates = !isRemove ? templates.Where(o => o.IsRemove != true) : templates;

        EmployeeViewModel model = new()
        {
            SprEmployeesMessageTemplateList = templates,
            PageInfo = new PageInfo
            {
                MaxPageList = 5,
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = templates.Count()
            },
            EmployeeId = employee.Id,
        };
        return PartialView("AppealDetails/Comments/MessageTemplates/PartialTableMessageTemplates", model);
    }

    #endregion

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  История изменений  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

    public IActionResult PartialTableChangeLogs(Guid appealId, string search, int page = 1)
    {
        ViewBag.AppealId = appealId;
        ViewBag.Search = search;
        var dataChangeLogs = _repository.DataChangeLog.Where(dcv => dcv.RowId == appealId);
        dataChangeLogs = string.IsNullOrEmpty(search) ? dataChangeLogs :
            search.ToLower().Split().Aggregate(dataChangeLogs, (current, item) => current.Where(h => h.FieldName.ToLower().Contains(item) || h.TableName.ToLower().Contains(item)));
        AppealViewModel model = new AppealViewModel
        {
            DataChangeLogList = dataChangeLogs.OrderByDescending(a => a.DateChange)/*.Skip((page - 1) * PageSize).Take(PageSize).ToList()*/,
            PageInfo = new PageInfo
            {
                MaxPageList = 5,
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataChangeLogs.Count()
            },
        };
        return PartialView("AppealDetails/ChangeLogs/PartialTableChangeLogs", model);
    }

    #endregion

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Email  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

    public IActionResult PartialTableEmails(Guid appealId, string search, int page = 1)
    {
        ViewBag.Search = search;
        ViewBag.Email_ = _repository.DataAppeal.Where(w => w.Id == appealId).FirstOrDefault()?.Email;
        var employeeId = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name)?.Id ?? Guid.Empty;
        var dataEmails = _repository.DataAppealEmail.Where(w => w.DataAppealId == appealId);
        dataEmails = string.IsNullOrEmpty(search) ? dataEmails :
            search.ToLower().Split().Aggregate(dataEmails, (current, item) => current.Where(h => h.Email.ToLower().Contains(item) || h.TextEmail.ToLower().Contains(item) || h.Caption.ToLower().Contains(item))).OrderByDescending(o => o.DateEmail);
        AppealViewModel model = new()
        {
            DataAppealId = appealId,
            DataAppealEmailList = dataEmails.OrderByDescending(a => a.DateEmail).Skip((page - 1) * PageSize).Take(PageSize).ToList(),
            DataAppealEmailListCount = dataEmails.Count(),
            CurrentEmployeeId = _repository.DataAppeal.Where(w => w.Id == appealId).FirstOrDefault()?.SprEmployeesIdCurrent ?? Guid.Empty,
            EmployeeId = employeeId,
            PageInfo = new PageInfo
            {
                MaxPageList = 5,
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataEmails.Count()
            },
        };
        ViewBag.SprEmployeesMessageTemplates = new SelectList(_repository.SprEmployeesMessageTemplate.Where(w => w.IsRemove != true && w.SprEmployeesId == employeeId).OrderByDescending(o => o.Sort), "Id", "MessageText");
        return PartialView("AppealDetails/Emails/PartialTableEmails", model);
    }

    /// <summary>
    /// Модальное окно для прикрепления почтового сообщения
    /// </summary>
    /// <returns>частичное представление модального окна</returns>
    [HttpPost]
    public IActionResult PartialModalAttachEmail(Guid appealId, string search)
    {
        //List<DataAppealEmail> noAttachEmails = _repository.DataAppealEmail.Where(w=>w.DataAppealId==null).ToList();
        var noAttachEmails = _repository.DataAppealEmail.Where(w => w.DataAppealId == null);
        noAttachEmails = string.IsNullOrEmpty(search) ? noAttachEmails :
            search.ToLower().Split().Aggregate(noAttachEmails, (current, item) => current.Where(h => h.Email.ToLower().Contains(item) || h.TextEmail.ToLower().Contains(item) || h.Caption.ToLower().Contains(item))).OrderByDescending(o => o.DateEmail);

        ViewBag.AppealId = appealId;
        ViewBag.SearchAttach = search;
        return PartialView("AppealDetails/Emails/PartialModalAttachEmail", noAttachEmails);
    }

    public IActionResult SubmitAttachEmail(Guid appealId, Guid emailId, bool attach)
    {
        DataAppealEmail model = _repository.DataAppealEmail.Where(w => w.Id == emailId).SingleOrDefault();
        if (attach)
        { model.DataAppealId = appealId; }
        else { model.DataAppealId = null; }
        _repository.Update(model);
        return RedirectToAction("PartialTableEmails", new { appealId = appealId });
    }

    /// <summary>
    /// Модальное окно для отправки почтового сообщения
    /// </summary>
    /// <returns>частичное представление модального окна</returns>
    [HttpPost]
    public IActionResult PartialModalSendEmail(Guid appealId)
    {
        var employee = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);
        DataAppealEmail model = new()
        {
            DataAppealId = appealId,
            Email = _repository.DataAppeal.Where(w => w.Id == appealId).SingleOrDefault().Email,
            SprEmployeesId = employee?.Id ?? Guid.Empty,
            EmployeesNameAdd = employee?.EmployeesName ?? "-"
        };
        return PartialView("AppealDetails/Emails/PartialModalSendEmail", model);
    }

    public IActionResult SubmitEmailSave(DataAppealEmail dataAppealEmail)
    {
        string result = SendEmail(dataAppealEmail);
        if (result == "OK")
        {
            var employee = _repository.SprEmployees.Include(i => i.SprEmployeesJobPos).Include(ii => ii.SprEmployeesDepartment).SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);
            dataAppealEmail.EmployeesNameAdd = employee?.EmployeesName ?? "";
            dataAppealEmail.SprEmployeesId = employee?.Id ?? Guid.Empty;
            dataAppealEmail.DateEmail = DateTime.Now;
            dataAppealEmail.EmailType = 1;
            _repository.Insert(dataAppealEmail);
            return RedirectToAction("PartialTableEmails", new { appealId = dataAppealEmail.DataAppealId });
        }
        else
        {
            throw new Exception("Ошибка ошибка отправки письма!");
        }
    }
    public string SendEmail(DataAppealEmail dataAppealEmail)
    {
        MailMessage mail = new();
        mail.From = new MailAddress("bega-m@yandex.ru"); // Адрес отправителя
        mail.To.Add(new MailAddress(dataAppealEmail.Email)); // Адрес получателя
        mail.Subject = dataAppealEmail.Caption;
        mail.Body = dataAppealEmail.TextEmail;

        SmtpClient client = new SmtpClient();

        client.Host = "smtp.yandex.ru";
        client.Port = 587;
        client.EnableSsl = true;
        client.Credentials = new System.Net.NetworkCredential("bega-m", "000"); // Ваши логин и пароль

        try
        {
            client.Send(mail);
            return "OK";
        }
        catch (Exception ex)
        {
            ex.ToString();
            return "Error";
        }
    }

    #endregion

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  Call  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

    public IActionResult PartialTableCalls(Guid appealId, string search, int page = 1)
    {
        ViewBag.Search = search;
        var employeeId = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name)?.Id ?? Guid.Empty;
        var dataCalls = _repository.DataAppealCall.Where(w => w.DataAppealId == appealId);
        AppealViewModel model = new()
        {
            DataAppealId = appealId,
            DataAppealCallList = dataCalls.OrderByDescending(a => a.DateCall),//.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
            DataAppealCallListCount = dataCalls.Count(),
            CurrentEmployeeId = _repository.DataAppeal.Where(w => w.Id == appealId).FirstOrDefault()?.SprEmployeesIdCurrent ?? Guid.Empty,
            EmployeeId = employeeId,
            PageInfo = new PageInfo
            {
                MaxPageList = 5,
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataCalls.Count()
            },
        };
        return PartialView("AppealDetails/Calls/PartialTableCalls", model);
    }

    /// <summary>
    /// Модальное окно для прикрепления звонка
    /// </summary>
    /// <returns>Частичное представление модального окна</returns>
    [HttpPost]
    public IActionResult PartialModalAttachCall(Guid appealId, string search)
    {
        IEnumerable<DataAppealCall> noAttachCalls = _repository.DataAppealCall.Where(w => w.DataAppealId == null);
        noAttachCalls = String.IsNullOrEmpty(search) ? noAttachCalls :
            search.ToLower().Split().Aggregate(noAttachCalls, (current, item) => current.Where(h => h.PhoneNumber.ToLower().Contains(item) || h.DateCall.ToString().Contains(item)));

        ViewBag.AppealId = appealId;
        ViewBag.AppealNumber = _repository.DataAppeal.Where(w => w.Id == appealId).FirstOrDefault().NumberAppeal;
        ViewBag.SearchAttachCall = search;
        return PartialView("AppealDetails/Calls/PartialModalAttachCall", noAttachCalls.OrderByDescending(o => o.DateCall));
    }

    public IActionResult SubmitAttachCall(Guid appealId, Guid callId, bool attach)
    {
        DataAppealCall model = _repository.DataAppealCall.Where(w => w.Id == callId).SingleOrDefault();
        if (attach)
        {
            model.DataAppealId = appealId;
            _repository.Update(model);
            return RedirectToAction("AppealInfo", new { number = _repository.DataAppeal.Where(w => w.Id == appealId).FirstOrDefault().NumberAppeal, modal = true });
        }
        else
        {
            model.DataAppealId = null;
            _repository.Update(model);
            return RedirectToAction("PartialTableCalls", new { appealId = appealId });
        }
    }
    public IActionResult AppealModalCall(Guid? appealId, string PhoneNumber, string appeal_number)
    {
        if (appealId != null)
        {
            DataAppeal model = _repository.DataAppeal.Where(w => w.Id == appealId).SingleOrDefault();
            return PartialView("AppealDetails/Calls/PartialModalCallTo", model);
        }
        else
        {
            if (appeal_number != null && appeal_number != "")
            {
                var model = _repository.DataAppeal.Where(w => w.NumberAppeal == appeal_number).FirstOrDefault();
                return PartialView("AppealDetails/Calls/PartialModalCallTo", model);
            }
            else
            {
                var appeal = _repository.DataAppeal.Where(w => w.PhoneNumber == PhoneNumber && w.ApplicantName.Length > 3).FirstOrDefault();
                DataAppeal model = new()
                {
                    PhoneNumber = PhoneNumber,
                    ApplicantName = appeal != null ? appeal.ApplicantName : "-"
                };
                return PartialView("AppealDetails/Calls/PartialModalCallTo", model);
            }
        }
    }

    [HttpPost]
    public string _CallPhone(Guid? appealId, string PhoneNumber, string NumberAppeal)
    {
        var DataAppeal = _repository.DataAppeal.Where(w => w.NumberAppeal == NumberAppeal).SingleOrDefault();
        var emp = _repository.SprEmployees.Where(w => w.EmployeesLogin == SignInManager.Context.User.Identity.Name).SingleOrDefault();
        string new_phone = PhoneNumber;
        if (PhoneNumber != null && PhoneNumber.Length >= 5)
        {
            new_phone = PhoneNumber;
        }
        else
        {
            new_phone = DataAppeal?.PhoneNumber;
        }
        string _testCALL = $"{{\"CaseNumber\":\"{(DataAppeal != null ? DataAppeal.NumberAppeal : null)}\"" +
            $",\"IdEmployee\":\"{emp.Id}\"" +
            $",\"EmployeeFio\":\"{emp.EmployeesName}\"" +
            $",\"CustomerName\":\"{(DataAppeal != null ? DataAppeal.ApplicantName : "Заявитель не найден")}\"" +
            $",\"CustomerPhone\":\"{new_phone}\"" +
            $",\"CallDuration\":\"0\"" +
            $",\"IdService\":\"{(DataAppeal != null ? DataAppeal.Id.ToString() : null)}\"" +
            $",\"IdFtp\":\"{Guid.Empty}\"" +
            $",\"IdMfc\":\"{emp.SprEmployeesDepartmentId}\"" +
            $",\"CallType\":\"{1}\"" +
            ",\"command\":\"" + 1 + "\"}";
        return _testCALL;
    }
    [HttpPost]
    public string _CallBackPhone(Guid CallbackId, string CustomerFio, string tel)
    {
        var emp = _repository.SprEmployees.Where(w => w.EmployeesLogin == SignInManager.Context.User.Identity.Name).SingleOrDefault();
        string _testCALL = $"{{\"CaseNumber\":\"{null}\"" +
            $",\"IdEmployee\":\"{emp.Id}\"" +
            $",\"EmployeeFio\":\"{emp.EmployeesName}\"" +
            $",\"CustomerName\":\"{CustomerFio}\"" +
            $",\"CustomerPhone\":\"{tel}\"" +
            $",\"CallDuration\":\"00:00:00\"" +
            $",\"IdService\":\"{CallbackId}\"" +
            $",\"IdFtp\":\"{Guid.Empty}\"" +
            $",\"IdMfc\":\"{emp.SprEmployeesDepartmentId}\"" +
            $",\"CallType\":\"{2}\"" +
            ",\"command\":\"" + 1 + "\"}";

        return _testCALL;
        //return Encoding.Default.GetString(CRPassword.Jitsi_encode(_testCALL));
    }

    #endregion

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  SMS  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

    public IActionResult PartialTableSms(Guid appealId, string search, int page = 1)
    {

        ViewBag.Search = search;
        var employeeId = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name)?.Id ?? Guid.Empty;
        var dataSms = _repository.DataAppealMessage.Where(w => w.DataAppealId == appealId);
        dataSms = string.IsNullOrEmpty(search) ? dataSms :
            search.ToLower().Split().Aggregate(dataSms, (current, item) => current.Where(h => h.EmployeesNameAdd.ToLower().Contains(item) || h.PhoneNumber.ToLower().Contains(item))).OrderByDescending(o => o.DateMessage);
        AppealViewModel model = new()
        {
            DataAppealId = appealId,
            DataAppealSmsList = dataSms.OrderByDescending(a => a.DateMessage).Skip((page - 1) * PageSize).Take(PageSize).ToList(),
            DataAppealSmsListCount = dataSms.Count(),
            PageInfo = new PageInfo
            {
                MaxPageList = 5,
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataSms.Count()
            },
        };
        return PartialView("AppealDetails/Sms/PartialTableSms", model);
    }

    /// <summary>
    /// Модальное окно для прикрепления смс
    /// </summary>
    /// <returns>Частичное представление модального окна</returns>
    [HttpPost]
    public IActionResult PartialModalAttachSms(Guid appealId, string search)
    {
        IEnumerable<DataAppealMessage> noAttachSms = _repository.DataAppealMessage.Where(w => w.DataAppealId == null);
        noAttachSms = String.IsNullOrEmpty(search) ? noAttachSms :
            search.ToLower().Split().Aggregate(noAttachSms, (current, item) => current.Where(h => h.EmployeesNameAdd.ToLower().Contains(item) || h.PhoneNumber.ToLower().Contains(item))).OrderByDescending(o => o.DateMessage);

        ViewBag.AppealId = appealId;
        ViewBag.SearchAttachSms = search;
        return PartialView("AppealDetails/Sms/PartialModalAttachSms", noAttachSms);
    }

    public IActionResult SubmitAttachSms(Guid appealId, Guid smsId, bool attach)
    {
        DataAppealMessage model = _repository.DataAppealMessage.Where(w => w.Id == smsId).SingleOrDefault();
        if (attach)
        { model.DataAppealId = appealId; }
        else { model.DataAppealId = null; }
        _repository.Update(model);
        return RedirectToAction("PartialTableSms", new { appealId = appealId });
    }

    #endregion

    #region |-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-[  File  ]-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|-=|=-|

    public IActionResult PartialTableFiles(Guid appealId, string search, int page = 1, bool isRemove = false)
    {
        ViewBag.Search = search;
        ViewBag.IsRemove = isRemove;
        var employeeId = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name)?.Id ?? Guid.Empty;
        var dataFiles = _repository.DataAppealFile.Where(w => w.DataAppealId == appealId);
        dataFiles = String.IsNullOrEmpty(search) ? dataFiles :
            search.ToLower().Split().Aggregate(dataFiles, (current, item) => current.Where(h => h.EmployeesNameAdd.ToLower().Contains(item) || h.FileName.ToLower().Contains(item))).OrderByDescending(o => o.DateAdd);
        dataFiles = !isRemove ? dataFiles.Where(o => o.IsRemove != true) : dataFiles;
        AppealViewModel model = new()
        {
            DataAppealId = appealId,
            DataAppealFileList = dataFiles.OrderByDescending(a => a.DateAdd)./*Skip((page - 1) * PageSize).Take(PageSize).*/ToList(),
            DataAppealFileListCount = dataFiles.Count(),
            CurrentEmployeeId = _repository.DataAppeal.Where(w => w.Id == appealId).FirstOrDefault()?.SprEmployeesIdCurrent ?? Guid.Empty,
            EmployeeId = employeeId,
            PageInfo = new PageInfo
            {
                MaxPageList = 5,
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataFiles.Count()
            },
        };
        return PartialView("AppealDetails/Files/PartialTableFiles", model);
    }

    /// <summary>
    /// Модальное окно для прикрепления звонка
    /// </summary>
    /// <returns>Частичное представление модального окна</returns>
    [HttpPost]
    public IActionResult PartialModalAddFile(Guid appealId)
    {
        ViewBag.AppealId = appealId;
        ViewBag.AppealNumber = _repository.DataAppeal.Where(w => w.Id == appealId).FirstOrDefault().NumberAppeal;
        return PartialView("AppealDetails/Files/PartialModalAddFile");
    }

    [HttpPost]
    public IActionResult SubmitAddFile(Guid appealId, IFormFile file)
    {
        if (file != null)
        {
            string theFileName = Path.GetFileNameWithoutExtension(file.FileName);

            byte[] thePictureAsbytes = new byte[file.Length];
            using (MemoryStream memoryStream = new())
            {
                file.CopyToAsync(memoryStream);
                thePictureAsbytes = memoryStream.ToArray();
            }

            var employee = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);
            var employeeId = employee?.Id ?? Guid.Empty;
            DataAppealFile saveModel = new() // Модель для сохранения в таблицу 
            {
                DataAppealId = appealId,
                FileName = theFileName,
                FileExt = Path.GetExtension(file.FileName),
                FileSize = thePictureAsbytes.Length,
                EmployeesNameAdd = employee.EmployeesName,
                SprEmployeesId = employeeId,
                DateAdd = DateTime.Now
            };
            _repository.Insert(saveModel); //Запись в таблицу 

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
            var res = ftp.UploadFileFtp(thePictureAsbytes, ftpModel.ftpServer, ftpModel.ftpLogin, ftpModel.ftpPass, saveModel.Id + saveModel.FileExt);
            return Json(res);
        }
        return Json("Файл не выбран");
    }
    [HttpPost]
    public IActionResult SubmitFileRecovery(Guid fileId)
    {
        var employee = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);
        DataAppealFile recoveryFile = _repository.DataAppealFile.SingleOrDefault(so => so.Id == fileId);
        recoveryFile.IsRemove = false;
        recoveryFile.EmployeesNameModify = employee.EmployeesName;
        _repository.Update(recoveryFile);
        return RedirectToAction("PartialTableFiles", new { appealId = recoveryFile.DataAppealId });
    }
    [HttpPost]
    public IActionResult SubmitFileDelete(Guid fileId)
    {
        var employee = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);
        DataAppealFile deleteFile = _repository.DataAppealFile.SingleOrDefault(so => so.Id == fileId);
        deleteFile.IsRemove = true;
        deleteFile.EmployeesNameModify = employee.EmployeesName;
        _repository.Update(deleteFile);
        return RedirectToAction("PartialTableFiles", new { appealId = deleteFile.DataAppealId });
    }
    #endregion

}