using ReceptionCentreNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using ReceptionCentreNew.Data.Context.App.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Ajax.Utilities;
using System.Security.Claims;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Routing;
using NuGet.Protocol.Core.Types;

namespace ReceptionCentreNew.Controllers;
public class ReportController : Controller
{
    private IRepository _repository;
    public SignInManager<ApplicationUser> SignInManager;
    public string EmploeeFio;
    public ReportController(IRepository repo, SignInManager<ApplicationUser> signInManager)
    {
        SignInManager = signInManager;
        EmploeeFio = SignInManager.Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;
        _repository = repo;

        ExcelPackage.LicenseContext = LicenseContext.Commercial;
    }
    public IActionResult Index()
    {
        return View();
    }

    #region ReportAppeals
    public IActionResult ReportAppeals(ReportParameters parameters)
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

        return View("ReportAppeals/_Page", parameters);
    }

    public IActionResult ReportAppealsTable(ReportParameters parameters)
    { 
        var appeals = 
            _repository.FuncDataAppealSelect(
           spr_employee_id:             parameters.SprEmployeeId,
           in_date_start:               parameters.DateStart,
           in_date_stop:                parameters.DateStop,
           in_spr_type_id:              parameters.SprTypeId,
           in_SprTypeDifficulty_id:     parameters.SprTypeDifficultyId,
           in_spr_category_id:          parameters.SprCategoryId,
           in_spr_subject_treatment_id: parameters.SprSubjectId,
           in_spr_status_id:            parameters.SprStatusId).OrderByDescending(o => o.OutDateAdd);

        AppealViewModel model = new()
        {
            DataAppealSelectList = appeals.OrderByDescending(o => o.OutDateAdd)
        }; 
        ViewBag.Employee = parameters.SprEmployeeId != null ? _repository.SprEmployees.Where(w => w.Id == parameters.SprEmployeeId).FirstOrDefault().EmployeesName : "Все";
        ViewBag.Type = parameters.SprTypeId != null ? _repository.SprType.Where(w => w.Id == parameters.SprTypeId).FirstOrDefault().TypeName : "Все";
        ViewBag.TypeDifficulty = parameters.SprTypeDifficultyId != null ? _repository.SprTypeDifficulty.Where(w => w.Id == parameters.SprTypeDifficultyId).FirstOrDefault().TypeName : "Все";
        ViewBag.Category = parameters.SprCategoryId != null ? _repository.SprCategory.Where(w => w.Id == parameters.SprCategoryId).FirstOrDefault().CategoryName : "Все";
        ViewBag.Subject = parameters.SprSubjectId != null ? _repository.SprSubjectTreatment.Where(w => w.Id == parameters.SprSubjectId).FirstOrDefault().SubjectName : "Все";
        ViewBag.Status = parameters.SprStatusId != null ? _repository.SprStatus.Where(w => w.Id == parameters.SprStatusId).FirstOrDefault().StatusName : "Все"; 

        return PartialView("ReportAppeals/_Table", model);
    }


    public IActionResult DownloadExcelReportAppeals(ReportParameters parameters)
    { 
        var model = _repository.FuncDataAppealSelect(parameters.SprEmployeeId, parameters.DateStart, parameters.DateStop, parameters.SprTypeId, parameters.SprTypeDifficultyId, parameters.SprCategoryId, parameters.SprSubjectId, parameters.SprStatusId).OrderByDescending(o => o.OutDateAdd);
        
        var Employee = parameters.SprEmployeeId != null ? _repository.SprEmployees.Where(w => w.Id == parameters.SprEmployeeId).FirstOrDefault().EmployeesName : "Все";
        var Type = parameters.SprTypeId != null ? _repository.SprType.Where(w => w.Id == parameters.SprTypeId).FirstOrDefault().TypeName : "Все";
        var TypeDifficulty = parameters.SprTypeDifficultyId != null ? _repository.SprTypeDifficulty.Where(w => w.Id == parameters.SprTypeDifficultyId).FirstOrDefault().TypeName : "Все";
        var Category = parameters.SprCategoryId != null ? _repository.SprCategory.Where(w => w.Id == parameters.SprCategoryId).FirstOrDefault().CategoryName : "Все";
        var Subject = parameters.SprSubjectId != null ? _repository.SprSubjectTreatment.Where(w => w.Id == parameters.SprSubjectId).FirstOrDefault().SubjectName : "Все";
        var Status = parameters.SprStatusId != null ? _repository.SprStatus.Where(w => w.Id == parameters.SprStatusId).FirstOrDefault().StatusName : "Все"; 

        ExcelPackage pck = new(new FileInfo("wwwroot/excel/report/reestr_appeals.xlsx"));

        var ws = pck.Workbook.Worksheets["Реестр"];
        ws.Cells["K4"].Value = EmploeeFio;
        ws.Cells["K5"].Value = DateTime.Now.ToString("G"); ;
        ws.Cells["C4"].Value = $"{parameters.Period}";
        ws.Cells["C5"].Value = $"{Employee}";
        ws.Cells["C6"].Value = $"{Type}";
        ws.Cells["C7"].Value = $"{TypeDifficulty}";
        ws.Cells["E4"].Value = $"{Status}";
        ws.Cells["E5"].Value = $"{Category}";
        ws.Cells["E6"].Value = $"{Subject}";

        int index = 10;
        foreach (var report in model)
        {
            ws.Cells["A" + index].Value = index - 9;
            ws.Cells["B" + index].Value = report.OutApplicantName;
            ws.Cells["C" + index].Value = report.OutPhoneNumber;
            ws.Cells["D" + index].Value = report.OutStatusName;
            ws.Cells["E" + index].Value = report.OutDateAdd;
            ws.Cells["F" + index].Value = report.OutMfcName;
            ws.Cells["G" + index].Value = report.OutNumberAppeal;
            ws.Cells["H" + index].Value = report.OutType;
            ws.Cells["I" + index].Value = report.OutTypeDifficulty;
            ws.Cells["J" + index].Value = report.OutSubjectTreatment;
            ws.Cells["K" + index].Value = report.OutCategory;
            ws.InsertRow(++index, 1, 10);
        }

        var memoryStream = new MemoryStream();
        pck.SaveAs(memoryStream);
        memoryStream.Position = 0;
        return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Реестр_обращений.xlsx");

    }
    #endregion

    #region StatisticsAppeals
    public IActionResult StatisticsAppeals(ReportParameters parameters)
    {
        var employees = _repository.SprEmployees.Where(e => e.IsRemove != true).OrderBy(o => o.EmployeesName);
        if (!User.IsInRole("superadmin") && !User.IsInRole("admin") && !User.IsInRole("operator"))
        {
            employees = employees.Where(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name).OrderBy(o => o.EmployeesName);
        }
        Guid empId = employees.Where(w => w.EmployeesLogin == SignInManager.Context.User.Identity.Name).FirstOrDefault().Id;
        ViewBag.SprEmployeesId = empId;
        ViewBag.SprEmployees = new SelectList(employees, "Id", "EmployeesName", empId);
        return View("StatisticsAppeals/_Page");
    }

    public IActionResult StatisticsAppealsTable(ReportParameters parameters)
    { 
        var appeals = _repository.FuncDataAppealSelect(parameters.SprEmployeeId, parameters.DateStart, parameters.DateStop, parameters.SprTypeId, parameters.SprTypeDifficultyId, parameters.SprCategoryId, parameters.SprSubjectId, parameters.SprStatusId).OrderByDescending(o => o.OutDateAdd);
        AppealViewModel model = new()
        {
            DataAppealSelectList = appeals.OrderByDescending(o => o.OutDateAdd)
        }; 
        ViewBag.Employee = parameters.SprEmployeeId != null ? _repository.SprEmployees.Where(w => w.Id == parameters.SprEmployeeId).FirstOrDefault().EmployeesName : "Все"; 
        return PartialView("StatisticsAppeals/_Table", model);
    }
    public IActionResult DownloadExcelStatisticAppeals(ReportParameters parameters)
    { 
        var model = _repository.FuncDataAppealSelect(parameters.SprEmployeeId, parameters.DateStart, parameters.DateStop, parameters.SprTypeId, parameters.SprTypeDifficultyId, parameters.SprCategoryId, parameters.SprSubjectId, parameters.SprStatusId).OrderByDescending(o => o.OutDateAdd);
     
        var Employee = parameters.SprEmployeeId != null ? _repository.SprEmployees.Where(w => w.Id == parameters.SprEmployeeId).FirstOrDefault().EmployeesName : "Все";
        var Type = parameters.SprTypeId != null ? _repository.SprType.Where(w => w.Id == parameters.SprTypeId).FirstOrDefault().TypeName : "Все";
        var TypeDifficulty = parameters.SprTypeDifficultyId != null ? _repository.SprTypeDifficulty.Where(w => w.Id == parameters.SprTypeDifficultyId).FirstOrDefault().TypeName : "Все";
        var Category = parameters.SprCategoryId != null ? _repository.SprCategory.Where(w => w.Id == parameters.SprCategoryId).FirstOrDefault().CategoryName : "Все";
        var Subject = parameters.SprSubjectId != null ? _repository.SprSubjectTreatment.Where(w => w.Id == parameters.SprSubjectId).FirstOrDefault().SubjectName : "Все";
        var Status = parameters.SprStatusId != null ? _repository.SprStatus.Where(w => w.Id == parameters.SprStatusId).FirstOrDefault().StatusName : "Все"; 

        ExcelPackage pck = new(new FileInfo("wwwroot/excel/report/statistic_appeals.xlsx"));

        var ws = pck.Workbook.Worksheets["Реестр"];
        ws.Cells["F4"].Value = EmploeeFio;
        ws.Cells["F5"].Value = DateTime.Now.ToString("G");
        ws.Cells["B4"].Value = $"{parameters.Period}";
        ws.Cells["B5"].Value = $"{Employee}";
        ws.Cells["B6"].Value = $"{Type}";
        ws.Cells["B7"].Value = $"{TypeDifficulty}";
        ws.Cells["D4"].Value = $"{Status}";
        ws.Cells["D5"].Value = $"{Category}";
        ws.Cells["D6"].Value = $"{Subject}";

        int index = 10;
        //--------------------------------------
        ws.InsertRow(index, 1, 9);
        ws.Cells["A" + index].Value = "Статусы";
        ws.Cells["C" + index].Value = "";
        ++index;
        foreach (var item in model.GroupBy(g => g.OutStatusName))
        {
            ws.Cells["A" + index].Value = item.Key;
            ws.Cells["C" + index].Value = item.Count();
            ws.InsertRow(++index, 1, index - 1);
        }
        //--------------------------------------
        ws.InsertRow(index, 1, 9);
        ws.Cells["A" + index].Value = "Типы";
        ws.Cells["C" + index].Value = "";
        ++index;
        foreach (var item in model.GroupBy(g => g.OutType))
        {
            ws.Cells["A" + index].Value = item.Key;
            ws.Cells["C" + index].Value = item.Count();
            ws.InsertRow(++index, 1, index - 1);
        }
        //--------------------------------------
        ws.InsertRow(index, 1, 9);
        ws.Cells["A" + index].Value = "Типы сложности";
        ws.Cells["C" + index].Value = "";
        ++index;
        foreach (var item in model.GroupBy(g => g.OutTypeDifficulty))
        {
            ws.Cells["A" + index].Value = item.Key;
            ws.Cells["C" + index].Value = item.Count();
            ws.InsertRow(++index, 1, index - 1);
        }
        //--------------------------------------
        ws.InsertRow(index, 1, 9);
        ws.Cells["A" + index].Value = "Предмет обращения";
        ws.Cells["C" + index].Value = "";
        ++index;
        foreach (var item in model.GroupBy(g => g.OutSubjectTreatment))
        {
            ws.Cells["A" + index].Value = item.Key;
            ws.Cells["C" + index].Value = item.Count();
            ws.InsertRow(++index, 1, index - 1);
        }
        //--------------------------------------
        ws.InsertRow(index, 1, 9);
        ws.Cells["A" + index].Value = "Категории";
        ws.Cells["C" + index].Value = "";
        ++index;
        foreach (var item in model.GroupBy(g => g.OutCategory))
        {
            ws.Cells["A" + index].Value = item.Key;
            ws.Cells["C" + index].Value = item.Count();
            ws.InsertRow(++index, 1, index - 1);
        }
        //--------------------------------------
        ws.InsertRow(index, 1, 9);
        ws.Cells["A" + index].Value = "Всего";
        ws.Cells["C" + index].Value = model.Count();

        ws.DeleteRow(index + 1);

        var memoryStream = new MemoryStream();

        pck.SaveAs(memoryStream);
        memoryStream.Position = 0;
        return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Статистика_обращений.xlsx");

    }
    #endregion

    #region ReportCalls
    public IActionResult ReportCalls(ReportParameters parameters)
    {

        var employees = _repository.SprEmployees.Where(e => e.IsRemove != true).OrderBy(o => o.EmployeesName);
        if (!User.IsInRole("superadmin") && !User.IsInRole("admin") && !User.IsInRole("operator"))
        {
            employees = employees.Where(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name).OrderBy(o => o.EmployeesName);
        }
        Guid empId = employees.Where(w => w.EmployeesLogin == SignInManager.Context.User.Identity.Name).FirstOrDefault().Id;
        ViewBag.SprEmployeesId = empId;
        ViewBag.SprEmployees = new SelectList(employees, "Id", "EmployeesName", empId);
        return View("ReportCalls/_Page");
    }

    public IActionResult ReportCallsTable(ReportParameters parameters)
    { 
        var calls = _repository.FuncDataAppealCallSelect(parameters.SprEmployeeId, parameters.DateStart, parameters.DateStop, parameters.Type, parameters.IsConnected).ToArray();
        CallsViewModel model = new CallsViewModel
        {
            CallList = calls.OrderByDescending(o => o.OutDateCall),
        };
        ViewBag.Period = $"{parameters.DateStart} - {parameters.DateStop}";
        ViewBag.Employee = parameters.SprEmployeeId != null ? _repository.SprEmployees.Where(w => w.Id == parameters.SprEmployeeId).FirstOrDefault().EmployeesName : "Все";
        ViewBag.Type = parameters.Type != null ? parameters.Type == 2 ? "Входящие" : "Исходящие" : "Все";
        ViewBag.IsConnected = parameters.IsConnected != null ? parameters.IsConnected == 1 ? "Да" : "Нет" : "Все"; 
        return PartialView("ReportCalls/_Table", model);
    }


    public IActionResult DownloadExcelReportCalls(ReportParameters parameters)
    { 
        var model = _repository.FuncDataAppealCallSelect(parameters.SprEmployeeId, parameters.DateStart, parameters.DateStop, parameters.Type, parameters.IsConnected).ToArray();

        var Period = $"{parameters.DateStart} - {parameters.DateStop}";
        var Employee = parameters.SprEmployeeId != null ? _repository.SprEmployees.Where(w => w.Id == parameters.SprEmployeeId).FirstOrDefault().EmployeesName : "Все";
        var Type = parameters.Type != null ? parameters.Type == 2 ? "Входящие" : "Исходящие" : "Все";
        var IsConnected = parameters.IsConnected != null ? parameters.IsConnected == 1 ? "Да" : "Нет" : "Все"; 

        ExcelPackage pck = new(new FileInfo("wwwroot/excel/report/reestr_calls.xlsx"));

        var ws = pck.Workbook.Worksheets["Реестр"];
        ws.Cells["E4"].Value = EmploeeFio;
        ws.Cells["E5"].Value = DateTime.Now.ToString("G");
        ws.Cells["C4"].Value = $"{Period}";
        ws.Cells["C5"].Value = $"{Employee}";
        ws.Cells["C6"].Value = $"{Type}";
        ws.Cells["C7"].Value = $"{IsConnected}";

        int index = 10;
        foreach (var report in model.OrderByDescending(o => o.OutDateCall))
        {
            ws.Cells["A" + index].Value = index - 9;
            ws.Cells["B" + index].Value = report.OutPhoneNumber;
            ws.Cells["C" + index].Value = report.OutCallType == 2 ? "Входящие" : "Исходящие";
            ws.Cells["D" + index].Value = report.OutNumberAppeal;
            ws.Cells["E" + index].Value = report.OutDateCall.ToString();
            ws.Cells["F" + index].Value = report.OutTimeTalk;
            ws.InsertRow(++index, 1, 10);
        }

        var memoryStream = new MemoryStream();
        pck.SaveAs(memoryStream);
        memoryStream.Position = 0;
        return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Реестр_звонков.xlsx");

    }
    #endregion

    #region StatisticsCalls
    public IActionResult StatisticsCalls(ReportParameters parameters)
    {
        var employees = _repository.SprEmployees.Where(e => e.IsRemove != true).OrderBy(o => o.EmployeesName);
        if (!User.IsInRole("superadmin") && !User.IsInRole("admin") && !User.IsInRole("operator"))
        {
            employees = employees.Where(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name).OrderBy(o => o.EmployeesName);
        }

        Guid empId = employees.Where(w => w.EmployeesLogin == SignInManager.Context.User.Identity.Name).FirstOrDefault().Id;
        ViewBag.SprEmployeesId = empId;
        ViewBag.SprEmployees = new SelectList(employees, "Id", "EmployeesName", empId);
         
        return View("StatisticsCalls/_Page", parameters);
    }

    public IActionResult StatisticsCallsTable(ReportParameters parameters)
    { 
        var calls = _repository.FuncDataAppealCallSelect(parameters.SprEmployeeId, parameters.DateStart, parameters.DateStop, parameters.Type, parameters.IsConnected).ToArray();
        CallsViewModel model = new()
        {
            CallList = calls.OrderByDescending(o => o.OutDateCall),
        };
        ViewBag.Employee = parameters.SprEmployeeId != null ? _repository.SprEmployees.Where(w => w.Id == parameters.SprEmployeeId).FirstOrDefault().EmployeesName : "Все"; 
        return PartialView("StatisticsCalls/_Table", model);
    }

    public IActionResult DownloadExcelStatisticCalls(ReportParameters parameters)
    { 
        var model = _repository.FuncDataAppealCallSelect(parameters.SprEmployeeId, parameters.DateStart, parameters.DateStop, parameters.Type, parameters.IsConnected).ToArray();
        var Employee = parameters.SprEmployeeId != null ? _repository.SprEmployees.Where(w => w.Id == parameters.SprEmployeeId).FirstOrDefault().EmployeesName : "Все";
        var Type = parameters.Type != null ? parameters.Type == 2 ? "Входящие" : "Исходящие" : "Все";
        var IsConnected = parameters.IsConnected != null ? parameters.IsConnected == 1 ? "Да" : "Нет" : "Все";

        ExcelPackage pck = new(new FileInfo("wwwroot/excel/report/statistic_calls.xlsx"));

        var ws = pck.Workbook.Worksheets["Реестр"];
        ws.Cells["F4"].Value = EmploeeFio;
        ws.Cells["F5"].Value = DateTime.Now.ToString("G");
        ws.Cells["B4"].Value = parameters.Period;
        ws.Cells["B5"].Value = Employee;
        ws.Cells["B6"].Value = Type;
        ws.Cells["B7"].Value = IsConnected;


        int index = 10;
        //--------------------------------------
        ws.InsertRow(index, 1, 9);
        ws.Cells["A" + index].Value = "Типы";
        ws.Cells["C" + index].Value = "";
        ++index;

        foreach (var item in model.GroupBy(g => g.OutCallType))
        {
            ws.Cells["A" + index].Value = item.Key == 2 ? "Входящие" : "Исходящие"; ;
            ws.Cells["C" + index].Value = item.Count();
            ws.InsertRow(++index, 1, index - 1);
        }

        //--------------------------------------
        ws.InsertRow(index, 1, 9);
        ws.Cells["A" + index].Value = "Прикрепленные";
        ws.Cells["C" + index].Value = "";
        ++index;
        ws.InsertRow(index, 1, index + 1);
        ws.Cells["A" + index].Value = "Да";
        ws.Cells["C" + index].Value = model.Where(w => w.OutNumberAppeal != null).Count();
        ++index;
        ws.InsertRow(index, 1, index + 1);
        ws.Cells["A" + index].Value = "Нет";
        ws.Cells["C" + index].Value = model.Where(w => w.OutNumberAppeal == null).Count();
        ++index;
        //--------------------------------------
        ws.InsertRow(index, 1, 9);
        ws.Cells["A" + index].Value = "Всего";
        ws.Cells["C" + index].Value = model.Count();

        ws.DeleteRow(index + 1);

        var memoryStream = new MemoryStream();
        pck.SaveAs(memoryStream);
        memoryStream.Position = 0;
        return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Статистика_звонков.xlsx");

    }
    #endregion

    #region ReportCategory
    public IActionResult ReportCategory()
    {
        return View("ReportCategory/_Filter");
    }
    public IActionResult ReportCategoryTable(ReportParameters parameters)
    { 
        ReportViewModel model = new()
        {
            ReportCategoryList = _repository.FuncReportCategory(parameters.DateStart, parameters.DateStop)
        }; 
        return PartialView("ReportCategory/_Table", model);
    }

    public IActionResult DownloadExcelReportCategory(ReportParameters parameters)
    { 
        var model = _repository.FuncReportCategory(parameters.DateStart, parameters.DateStop).ToArray();

        var Period = $"{parameters.DateStart} - {parameters.DateStop}"; 

        var package = new ExcelPackage(new FileInfo("wwwroot/excel/report/report_category.xlsx"));

        var ws = package.Workbook.Worksheets["Реестр"];
        ws.Cells["F4"].Value = EmploeeFio;
        ws.Cells["F5"].Value = DateTime.Now.ToString("G");
        ws.Cells["C4"].Value = $"{Period}";

        int q, c, r, p, a;
        int total_q = model.Sum(s => s.OutCountQuestion);
        int total_c = model.Sum(s => s.OutCountClaim);
        int total_r = model.Sum(s => s.OutCountReview);
        int total_p = model.Sum(s => s.OutCountProposals);
        int total_a = model.Sum(s => s.OutCountAlert);
        int totalSum = total_q + total_c + total_r + total_p + total_a;

        int index = 8;
        foreach (var report in model)
        {
            q = report.OutCountQuestion;
            c = report.OutCountClaim;
            r = report.OutCountReview;
            p = report.OutCountProposals;
            a = report.OutCountAlert;

            ws.Cells["A" + index].Value = report.OutNum;
            ws.Cells["B" + index].Value = report.OutMfcName;
            ws.Cells["C" + index].Value = q;
            ws.Cells["D" + index].Value = c;
            ws.Cells["E" + index].Value = r;
            ws.Cells["F" + index].Value = p;
            ws.Cells["G" + index].Value = a;
            ws.Cells["H" + index].Value = q + c + r + p + a;

            ws.InsertRow(++index, 1, 8);
        }

        ws.Cells["A" + index].Value = "";
        ws.Cells["B" + index].Value = "Итого";
        ws.Cells["C" + index].Value = total_q;
        ws.Cells["D" + index].Value = total_c;
        ws.Cells["E" + index].Value = total_r;
        ws.Cells["F" + index].Value = total_p;
        ws.Cells["G" + index].Value = total_a;
        ws.Cells["H" + index].Value = totalSum;

        var memoryStream = new MemoryStream();
        package.SaveAs(memoryStream);
        memoryStream.Position = 0;
        return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Отчет_по_категориям.xlsx");

    }
    #endregion

    #region ReportTreatment
    public IActionResult ReportTreatment()
    {
        return View("ReportTreatment/_Filter");
    }
    public IActionResult ReportTreatmentTable(ReportParameters parameters)
    {
        ReportViewModel model = new()
        {
            ReportTreatmentList = _repository.FuncReportTreatment(parameters.DateStart, parameters.DateStop)
        }; 
        return PartialView("ReportTreatment/_Table", model);
    }

    public IActionResult DownloadExcelReportTreatment(ReportParameters parameters)
    {  
        var model = _repository.FuncReportTreatment(parameters.DateStart, parameters.DateStop).ToArray();

        var Period = $"{parameters.DateStart} - {parameters.DateStop}"; 

        ExcelPackage pck = new(new FileInfo("wwwroot/excel/report/report_treatment.xlsx"));

        var ws = pck.Workbook.Worksheets["Реестр"];
        ws.Cells["E4"].Value = EmploeeFio;
        ws.Cells["E5"].Value = DateTime.Now.ToString("G");
        ws.Cells["C4"].Value = $"{Period}";

        int e, m, c, o, q, s;
        int sum;
        int total_e = model.Sum(ss => ss.OutCountEarth);
        int total_s = model.Sum(ss => ss.OutCountSocial);
        int total_m = model.Sum(ss => ss.OutCountMigratory);
        int total_c = model.Sum(ss => ss.OutCountCommercial);
        int total_q = model.Sum(ss => ss.OutCountQuality);
        int total_o = model.Sum(ss => ss.OutCountOther);
        int totalSum = total_e + total_m + total_c + total_o + total_q + total_s;

        int index = 8;
        foreach (var report in model)
        {
            e = report.OutCountEarth;
            s = report.OutCountSocial;
            m = report.OutCountMigratory;
            c = report.OutCountCommercial;
            q = report.OutCountQuality;
            o = report.OutCountOther;
            sum = e + m + c + o + q + s;

            ws.Cells["A" + index].Value = report.OutNum;
            ws.Cells["B" + index].Value = report.OutMfcName;
            ws.Cells["C" + index].Value = report.OutCountEarth;
            ws.Cells["D" + index].Value = report.OutCountSocial;
            ws.Cells["E" + index].Value = report.OutCountMigratory;
            ws.Cells["F" + index].Value = report.OutCountCommercial;
            ws.Cells["G" + index].Value = report.OutCountQuality;
            ws.Cells["H" + index].Value = report.OutCountOther;
            ws.Cells["I" + index].Value = sum;
            ws.InsertRow(++index, 1, 8);
        }
        ws.Cells["A" + index].Value = "";
        ws.Cells["B" + index].Value = "Итого";
        ws.Cells["C" + index].Value = total_e;
        ws.Cells["D" + index].Value = total_s;
        ws.Cells["E" + index].Value = total_m;
        ws.Cells["F" + index].Value = total_c;
        ws.Cells["G" + index].Value = total_q;
        ws.Cells["H" + index].Value = total_o;
        ws.Cells["I" + index].Value = totalSum;
        ws.InsertRow(++index, 1, 7);

        var memoryStream = new MemoryStream();
        pck.SaveAs(memoryStream);
        memoryStream.Position = 0;
        return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Отчет_по_предмету_обращения.xlsx");
    }
    #endregion
}