using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Data.Context.App.Abstract;
using SmartBreadcrumbs.Attributes;

namespace ReceptionCentreNew.Controllers;
[Authorize]
public class SettingsController : Controller
{
    private readonly IRepository _repository;
    public SettingsController(IRepository repo)
    {
        _repository = repo;
    }

    [HttpGet]
    [Breadcrumb("Настройки", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))]
    public async Task<IActionResult> Index() =>
          View(await GetSettingsAsync()); 

    [HttpGet]
    public async Task<IActionResult> Edit(int settingId) =>
          PartialView(await GetSetting(settingId));

    [HttpPost]
    public async Task Edit(SprSetting setting) =>
        await _repository.Update(setting);

    public async Task<SprSetting?> GetSetting(int id) =>
        await _repository.SprSetting.Where(w => w.Id == id).SingleOrDefaultAsync();

    public async Task<List<SprSetting>> GetSettingsAsync() =>
        await _repository.SprSetting.AsNoTracking().ToListAsync();
}