using ReceptionCentreNew.Models;
using Microsoft.AspNetCore.Mvc;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Data.Context.App.Abstract;
using Microsoft.AspNetCore.Authorization;
using SmartBreadcrumbs.Attributes;
using Microsoft.EntityFrameworkCore;

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
            ErrorsList = errors.OrderByDescending(a => a.ErrorDate).OrderByDescending(d => d.ErrorDate).Take(200),
        };
        return PartialView("Errors/PartialTableErrors", model);
    }

    public IActionResult DeleteDuplicationCall()
    {
        _repository.FuncDeleteDublicationCall();
        return null;
    }

    public async Task<List<SprSetting>> GetSettingsAsync() => await _repository.SprSetting.AsNoTracking().ToListAsync();
}