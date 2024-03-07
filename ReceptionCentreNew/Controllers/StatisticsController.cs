using ReceptionCentreNew.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ReceptionCentreNew.Areas.Identity.User;
using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Data.Context.App.Abstract;

namespace ReceptionCentreNew.Controllers;
[ClientErrorHandler]
[Authorize]
public class StatisticsController : Controller
{
    private IRepository _repository;
    private string? UserName;
    public StatisticsController(IRepository repo, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _repository = repo;
        UserName = _repository.SprEmployees.First(s => s.EmployeesLogin == userManager.GetUserAsync(signInManager.Context.User).Result.Email).EmployeesName;
    }

    public IActionResult Statistics()
    {
        return View();
    }

    #region Main Chart
    public JsonResult GetChartInYear()
    {
        var data = _repository.FuncChartInYear();
        return Json(JsonConvert.SerializeObject(data));
    }
    public JsonResult GetChartInWeek()
    {
        var data = _repository.FuncChartInWeek().OrderBy(o => o.OutDate).Select(s => new { s.OutCountIncoming, s.OutCountOutgoing, OutDate = s.OutDate + " " + s.OutDayWeek });
        return Json(JsonConvert.SerializeObject(data));
    }

    public JsonResult GetChartClaimInYear()
    {
        var data = _repository.FuncChartClaimInYear();
        return Json(JsonConvert.SerializeObject(data));
    }
    public JsonResult GetChartClaimInWeek()
    {
        var data = _repository.FuncChartClaimInWeek().OrderBy(o => o.OutDate).Select(s => new { s.OutCountClaim, s.OutCountNotify, OutDate = s.OutDate.ToShortDateString() + " " + s.OutDayWeek });
        return Json(JsonConvert.SerializeObject(data));
    }
    public JsonResult GetChartClaimForMfc()
    {
        var data = _repository.FuncChartClaimForMfc().Where(w => w.OutMfcName != "Не выбран").OrderByDescending(o => o.OutCountClaim).Take(10).Select(s => new { s.OutCountClaim, s.OutCountNotify, OutDate = s.OutMfcName });
        return Json(JsonConvert.SerializeObject(data));
    }

    #endregion

    #region Menu Chart
    #region AppealsCount
    public IActionResult AppealsCount()
    {
        ViewBag.SprMfc = new SelectList(_repository.SprMfc.Where(e => e.IsRemove != true).OrderBy(o => o.MfcName), "id", "MfcName");
        ViewBag.SprTreatment = new SelectList(_repository.SprSubjectTreatment.Where(e => e.IsRemove != true).OrderBy(o => o.Sort), "id", "SubjectName");
        ViewBag.SprCategory = new SelectList(_repository.SprCategory.Where(e => e.IsRemove != true).OrderBy(o => o.Sort), "id", "CategoryName");
        ViewBag.SprType = new SelectList(_repository.SprType.Where(w => w.IsRemove != true).OrderBy(o => o.TypeName), "id", "TypeName");
        ViewBag.SprTypeDifficulty = new SelectList(_repository.SprTypeDifficulty.Where(e => e.IsRemove != true).OrderBy(o => o.TypeName), "id", "TypeName");

        return View("AppealsCount");
    }
    public JsonResult AppealsCountResult(Guid? SprMfcId, Guid? spr_treatment_id, Guid? SprCategoryId, Guid? SprTypeId, Guid? SprTypeDifficultyId)
    {
        var data = _repository.FuncStatisticsDataAppeal(SprMfcId, spr_treatment_id, SprCategoryId, SprTypeId, SprTypeDifficultyId).OrderBy(o => o.OutMonth).Select(s => new { OutCount = s.OutCountAppeal, OutDate = s.OutMonth + "." + s.OutYear });
        return Json(JsonConvert.SerializeObject(data));
    }

    #endregion

    #region AppealsCall
    public IActionResult AppealsCall()
    {
        return View("AppealsCall");
    }
    public JsonResult AppealsCallResult()
    {
        var data = _repository.FuncStatisticsDataAppealCall().OrderBy(o => o.OutMonth).Select(s => new { s.OutCountCallIncoming, s.OutCountCallOutgoing, s.OutCountCallMissed, OutDate = s.OutMonth + "." + s.OutYear }); ;
        return Json(JsonConvert.SerializeObject(data));
    }
    #endregion

    #region AppealsTreatment
    public IActionResult AppealsTreatment()
    {
        ViewBag.SprMfc = new SelectList(_repository.SprMfc.Where(e => e.IsRemove != true).OrderBy(o => o.MfcName), "id", "MfcName");
        return View("AppealsTreatment");
    }

    public JsonResult AppealsTreatmentResult(Guid? SprMfcId, DateTime dateStart, DateTime dateStop)
    {
        var data = _repository.FuncStatisticsDataAppealSubject(SprMfcId, dateStart, dateStop).Select(s => new { OutCount = s.OutCountAppeal, OutDate = s.OutSubjectName + " (" + s.OutCountAppeal + ")" }).OrderByDescending(o => o.OutCount);
        List<object[]> mass = new List<object[]>();
        foreach (var item in data)
        {
            object[] obj = new object[] { item.OutDate, item.OutCount };
            mass.Add(obj);
        }
        return Json(JsonConvert.SerializeObject(mass));
    }
    #endregion

    #region AppealsCategory
    public IActionResult AppealsCategory()
    {
        ViewBag.SprMfc = new SelectList(_repository.SprMfc.Where(e => e.IsRemove != true).OrderBy(o => o.MfcName), "id", "MfcName");

        return View("AppealsCategory");
    }

    public JsonResult AppealsCategoryResult(Guid? SprMfcId, DateTime dateStart, DateTime dateStop)
    {
        var data = _repository.FuncStatisticsDataAppealCategory(SprMfcId, dateStart, dateStop).Select(s => new { OutCount = s.OutCountAppeal, OutDate = s.OutCategoryName + " (" + s.OutCountAppeal + ")" }).OrderByDescending(o => o.OutCount);
        List<object[]> mass = new List<object[]>();
        foreach (var item in data)
        {
            object[] obj = new object[] { item.OutDate, item.OutCount };
            mass.Add(obj);
        }
        return Json(JsonConvert.SerializeObject(mass));
    }

    #endregion

    #region AppealsType
    public IActionResult AppealsType()
    {
        ViewBag.SprMfc = new SelectList(_repository.SprMfc.Where(e => e.IsRemove != true).OrderBy(o => o.MfcName), "id", "MfcName");

        return View("AppealsType");
    }

    public JsonResult AppealsTypeResult(Guid? SprMfcId, DateTime dateStart, DateTime dateStop)
    {
        var data = _repository.FuncStatisticsDataAppealType(SprMfcId, dateStart, dateStop).Select(s => new { OutCount = s.OutCountAppeal, OutDate = s.OutTypeName + " (" + s.OutCountAppeal + ")" }).OrderByDescending(o => o.OutCount);
        List<object[]> mass = new List<object[]>();
        foreach (var item in data)
        {
            object[] obj = new object[] { item.OutDate, item.OutCount };
            mass.Add(obj);
        }
        return Json(JsonConvert.SerializeObject(mass));
    }
    #endregion

    #region AppealsDifficulty
    public IActionResult AppealsTypeDifficulty()
    {
        ViewBag.SprMfc = new SelectList(_repository.SprMfc.Where(e => e.IsRemove != true).OrderBy(o => o.MfcName), "id", "MfcName");
        return View("AppealsTypeDifficulty");
    }

    public JsonResult AppealsTypeDifficultyResult(Guid? SprMfcId, DateTime dateStart, DateTime dateStop)
    {
        var data = _repository.FuncStatisticsDataAppealTypeDifficulty(SprMfcId, dateStart, dateStop).Select(s => new { OutCount = s.OutCountAppeal, OutDate = s.OutTypeName + " (" + s.OutCountAppeal + ")" }).OrderByDescending(o => o.OutCount);
        List<object[]> mass = new List<object[]>();
        foreach (var item in data)
        {
            object[] obj = new object[] { item.OutDate, item.OutCount };
            mass.Add(obj);
        }
        return Json(JsonConvert.SerializeObject(mass));
    }

    #endregion

    #endregion
}