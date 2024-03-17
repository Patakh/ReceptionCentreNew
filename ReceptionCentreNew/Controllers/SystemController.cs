using ReceptionCentreNew.Models;
using Microsoft.AspNetCore.Mvc;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Data.Context.App.Abstract;
using Microsoft.AspNetCore.Authorization;
using SmartBreadcrumbs.Attributes;
using System.Drawing.Printing;

namespace ReceptionCentreNew.Controllers;
[Authorize]
public class SystemController : Controller
{
    public int PageSize = 10;

    private readonly IRepository _repository;
    public SystemController(IRepository repo)
    {
        _repository = repo;
    }

    [HttpGet]
    [Breadcrumb("Настройки", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))]
    public IActionResult PartialTableSettings()
    {
        List<SprSetting> model = _repository.SprSetting.ToList();
        return View("~/Views/System/Settting/PartialTableSettings.cshtml", model);
    }

    [HttpGet]
    public IActionResult EditSetting(int settingId)
    {
        var model = _repository.SprSetting.Where(w => w.Id == settingId).SingleOrDefault();
        return PartialView("~/Views/System/Settting/PartialModalEditSettting.cshtml", model);
    }
    public IActionResult SubmitSettingSave(SprSetting setting)
    {
        _repository.Update(setting);
        return RedirectToAction("~/Views/System/Settting/PartialTableSettings.cshtml");
    }

    [Breadcrumb("Изменения", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))]
    public IActionResult ChangeLogs()
    {
        return View("ChangeLogs/Main");
    }
     
    public IActionResult PartialTableChangeLogs(string search)
    {
        ViewBag.Serach = search;
        var dataChangeLogs = _repository.DataChangeLog;
    
        ReferenceViewModel model = new()
        {
            DataChangeLogList = dataChangeLogs.OrderByDescending(a => a.DateChange).OrderByDescending(d => d.DateChange).Take(200).ToList(),
        };
        return PartialView("ChangeLogs/PartialTableChangeLogs", model);
    }

    [Breadcrumb("Ошибки", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))]
    public IActionResult Errors()
    {
        return View("Errors/Main");
    }

    public IActionResult PartialTableErrors(string search, int page = 1)
    {
        ViewBag.Serach = search;
        var errors = _repository.DataSystemErrors;
        errors = string.IsNullOrEmpty(search) ? errors :
            search.ToLower().Split().Aggregate(errors, (current, item) => current.Where(h => h.ErrorMessage.ToLower().Contains(item) || h.ErrorInnerException.ToLower().Contains(item) || h.EmployeesName.ToLower().Contains(item)));

        ReferenceViewModel model = new()
        {
            ErrorsList = errors.OrderByDescending(a => a.ErrorDate).OrderByDescending(d=>d.ErrorDate).Take(200),
        };
        return PartialView("Errors/PartialTableErrors", model);
    }

    public IActionResult DeleteDublicationCall()
    {
        _repository.FuncDeleteDublicationCall();
        return null;
    }
}