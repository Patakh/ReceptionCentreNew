using ReceptionCentreNew.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Models;
using ReceptionCentreNew.Data.Context.App.Abstract;
using SmartBreadcrumbs.Attributes;

namespace ReceptionCentreNew.Controllers;
[ClientErrorHandler]
[Authorize]
public class StatisticsController : Controller
{
    private IRepository _repository;
    private string? UserName;
    public StatisticsController(IRepository repo, SignInManager<ApplicationUser> signInManager)
    {
        _repository = repo;
        UserName = _repository.SprEmployees.First(s => s.EmployeesLogin == signInManager.Context.User.Identity.Name).EmployeesName;
    }

    public IActionResult Statistics()
    {
        return View();
    }

    public IActionResult GetChartInYear()
    {
        var data = _repository.FuncChartInYear();
        return Json(JsonConvert.SerializeObject(data));
    }
    public IActionResult GetChartInWeek()
    {
        var data = _repository.FuncChartInWeek().OrderBy(o => o.OutDate).Select(s => new { s.OutCountIncoming, s.OutCountOutgoing, OutDate = s.OutDate + " " + s.OutDayWeek });
        return Json(JsonConvert.SerializeObject(data));
    }

    public IActionResult GetChartClaimInYear()
    {
        var data = _repository.FuncChartClaimInYear();
        return Json(JsonConvert.SerializeObject(data));
    }
    public IActionResult GetChartClaimInWeek()
    {
        var data = _repository.FuncChartClaimInWeek().OrderBy(o => o.OutDate).Select(s => new { s.OutCountClaim, s.OutCountNotify, OutDate = s.OutDate.ToShortDateString() + " " + s.OutDayWeek });
        return Json(JsonConvert.SerializeObject(data));
    }
    public IActionResult GetChartClaimForMfc()
    {
        var data = _repository.FuncChartClaimForMfc().Where(w => w.OutMfcName != "Не выбран").OrderByDescending(o => o.OutCountClaim).Take(10).Select(s => new { s.OutCountClaim, s.OutCountNotify, OutDate = s.OutMfcName });
        return Json(JsonConvert.SerializeObject(data));
    }

    [Breadcrumb("Статистика \"Количество обращений\"", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))] 
    public IActionResult AppealsCount()
    {
        ViewBag.SprMfc = new SelectList(_repository.SprMfc.Where(e => e.IsRemove != true).OrderBy(o => o.MfcName), "Id", "MfcName");
        ViewBag.SprTreatment = new SelectList(_repository.SprSubjectTreatment.Where(e => e.IsRemove != true).OrderBy(o => o.Sort), "Id", "SubjectName");
        ViewBag.SprCategory = new SelectList(_repository.SprCategory.Where(e => e.IsRemove != true).OrderBy(o => o.Sort), "Id", "CategoryName");
        ViewBag.SprType = new SelectList(_repository.SprType.Where(w => w.IsRemove != true).OrderBy(o => o.TypeName), "Id", "TypeName");
        ViewBag.SprTypeDifficulty = new SelectList(_repository.SprTypeDifficulty.Where(e => e.IsRemove != true).OrderBy(o => o.TypeName), "Id", "TypeName");
        return View("AppealsCount");
    }
    public IActionResult AppealsCountResult(Guid? SprMfcId, Guid? SprTreatmentId, Guid? SprCategoryId, Guid? SprTypeId, Guid? SprTypeDifficultyId)
    {
        var json = _repository.FuncStatisticsDataAppeal(SprMfcId, SprTreatmentId, SprCategoryId, SprTypeId, SprTypeDifficultyId);
        var data = json.OrderBy(o => o.OutMonth).Select(s => new { OutCount = s.OutCountAppeal, OutDate = s.OutMonth + "." + s.OutYear });
        return Json(JsonConvert.SerializeObject(data));
    }

    [Breadcrumb("Статистика \"Количество звонков\"", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))] 
    public IActionResult AppealsCall()
    {
        return View("AppealsCall");
    }

    public IActionResult AppealsCallResult()
    {
        var data = _repository.FuncStatisticsDataAppealCall().OrderBy(o => o.OutMonth).Select(s => new { s.OutCountCallIncoming, s.OutCountCallOutgoing, s.OutCountCallMissed, OutDate = s.OutMonth + "." + s.OutYear }); ;
        return Json(JsonConvert.SerializeObject(data));
    }
      
    [Breadcrumb("Статистика \"Предмет обращения\"", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))] 
    public IActionResult AppealsTreatment()
    {
        ViewBag.SprMfc = new SelectList(_repository.SprMfc.Where(e => e.IsRemove != true).OrderBy(o => o.MfcName), "Id", "MfcName");
        return View("AppealsTreatment");
    }

    public IActionResult AppealsTreatmentResult(Guid? SprMfcId, DateTime DateStart, DateTime DateStop)
    {
        DateStop = DateStop.AddDays(1);
        if (DateStart == DateStop && DateStart == DateTime.Now.Date) DateStart = DateTime.Now;

        var data = _repository.FuncStatisticsDataAppealSubject(SprMfcId, DateStart, DateStop).Select(s => new { OutCount = s.OutCountAppeal, OutDate = s.OutSubjectName + " (" + s.OutCountAppeal + ")" }).OrderByDescending(o => o.OutCount);
        List<object[]> mass = new();
        foreach (var item in data)
        {
            object[] obj = [item.OutDate, item.OutCount];
            mass.Add(obj);
        }
        return Json(JsonConvert.SerializeObject(mass));
    }
     
    [Breadcrumb("Статистика \"Категория обращения\"", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))]
    public IActionResult AppealsCategory()
    {
        ViewBag.SprMfc = new SelectList(_repository.SprMfc.Where(e => e.IsRemove != true).OrderBy(o => o.MfcName), "Id", "MfcName");

        return View("AppealsCategory");
    }

    public IActionResult AppealsCategoryResult(Guid? SprMfcId, DateTime DateStart, DateTime DateStop)
    {
        DateStop = DateStop.AddDays(1);
        if (DateStart == DateStop && DateStart == DateTime.Now.Date) DateStart = DateTime.Now;

        var data = _repository.FuncStatisticsDataAppealCategory(SprMfcId, DateStart, DateStop).Select(s => new { OutCount = s.OutCountAppeal, OutDate = s.OutCategoryName + " (" + s.OutCountAppeal + ")" }).OrderByDescending(o => o.OutCount);
        List<object[]> mass = new();
        foreach (var item in data)
        {
            object[] obj = [item.OutDate, item.OutCount];
            mass.Add(obj);
        }
        return Json(JsonConvert.SerializeObject(mass));
    }

    [Breadcrumb("Статистика \"Тип обращения\"", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))]
    public IActionResult AppealsType()
    {
        ViewBag.SprMfc = new SelectList(_repository.SprMfc.Where(e => e.IsRemove != true).OrderBy(o => o.MfcName), "Id", "MfcName");

        return View("AppealsType");
    }

    public IActionResult AppealsTypeResult(Guid? SprMfcId, DateTime DateStart, DateTime DateStop)
    {
        DateStop = DateStop.AddDays(1);
        if (DateStart == DateStop && DateStart == DateTime.Now.Date) DateStart = DateTime.Now;

        var data = _repository.FuncStatisticsDataAppealType(SprMfcId, DateStart, DateStop).Select(s => new { OutCount = s.OutCountAppeal, OutDate = s.OutTypeName + " (" + s.OutCountAppeal + ")" }).OrderByDescending(o => o.OutCount);
        List<object[]> mass = new();
        foreach (var item in data)
        {
            object[] obj = [item.OutDate, item.OutCount];
            mass.Add(obj);
        }
        return Json(JsonConvert.SerializeObject(mass));
    }
     
    [Breadcrumb("Статистика \"Тип сложности\"", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))]
    public IActionResult AppealsTypeDifficulty()
    {
        ViewBag.SprMfc = new SelectList(_repository.SprMfc.Where(e => e.IsRemove != true).OrderBy(o => o.MfcName), "Id", "MfcName");
        return View("AppealsTypeDifficulty");
    }

    public IActionResult AppealsTypeDifficultyResult(Guid? SprMfcId, DateTime DateStart, DateTime DateStop)
    {
        DateStop = DateStop.AddDays(1);
        if (DateStart == DateStop && DateStart == DateTime.Now.Date) DateStart = DateTime.Now;
        var json = _repository.FuncStatisticsDataAppealTypeDifficulty(SprMfcId, DateStart, DateStop);
        var data = json.Select(s => new { OutCount = s.OutCountAppeal, OutDate = s.OutTypeName + " (" + s.OutCountAppeal + ")" }).OrderByDescending(o => o.OutCount);
        List<object[]> mass = new();
        foreach (var item in data)
        {
            object[] obj = [item.OutDate, item.OutCount];
            mass.Add(obj);
        }
        return Json(JsonConvert.SerializeObject(mass));
    }
}