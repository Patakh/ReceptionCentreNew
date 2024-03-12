using ReceptionCentreNew.Models;
using Microsoft.AspNetCore.Mvc;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Data.Context.App.Abstract;
using Microsoft.AspNetCore.Authorization;

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
    public IActionResult PartialTableSettings()
    {
        List<SprSetting> model = _repository.SprSetting.ToList();
        return View(model);
    }

    public IActionResult EditSetting(int settingId)
    {
        var model = _repository.SprSetting.Where(w => w.Id == settingId).SingleOrDefault();
        return PartialView("System/PartialModalEditSettting", model);
    }
    public IActionResult SubmitSettingSave(SprSetting setting)
    {
        _repository.Update(setting);
        return RedirectToAction("System/PartialTableSettings");
    }
    public IActionResult ChangeLogs()
    {
        return View("ChangeLogs/Main");
    }
    public IActionResult PartialTableChangeLogs(string search, int page = 1)
    {
        ViewBag.Serach = search;
        var dataChangeLogs = _repository.DataChangeLog;
        dataChangeLogs = string.IsNullOrEmpty(search) ? dataChangeLogs :
            search.ToLower().Split().Aggregate(dataChangeLogs, (current, item) => current.Where(h => h.FieldName.ToLower().Contains(item) || h.TableName.ToLower().Contains(item) || h.OldValue.ToLower().Contains(item) || h.NewValue.ToLower().Contains(item) || h.EmployeesName.ToLower().Contains(item)));

        ReferenceViewModel model = new()
        {
            DataChangeLogList = dataChangeLogs.OrderByDescending(a => a.DateChange).Skip((page - 1) * PageSize).Take(PageSize),
            PageInfo = new PageInfo
            {
                MaxPageList = 5,
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = dataChangeLogs.Count()
            },
        };
        return PartialView("ChangeLogs/PartialTableChangeLogs", model);
    }

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
            ErrorsList = errors.OrderByDescending(a => a.ErrorDate).Skip((page - 1) * PageSize).Take(PageSize),
            PageInfo = new PageInfo
            {
                MaxPageList = 5,
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = errors.Count()
            },
        };
        return PartialView("Errors/PartialTableErrors", model);
    }

    public IActionResult DeleteDublicationCall()
    {
        _repository.FuncDeleteDublicationCall();
        return null;
    }
}