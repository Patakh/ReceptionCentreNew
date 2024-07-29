using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Data.Context.App.Abstract;
using ReceptionCentreNew.Helpers.PageHelper;
using SmartBreadcrumbs.Attributes;

namespace ReceptionCentreNew.Controllers;
[Authorize]
public class SettingsController : Controller
{
    private readonly IRepository _repository; 
    private IPageHelper<SprSetting> _pageHelper;
    public SettingsController(IRepository repo, IPageHelper<SprSetting> pageHelper)
    {
        _pageHelper = pageHelper;
        _repository = repo;
    }

    [HttpGet]
    [Breadcrumb("Настройки", FromAction = nameof(HomeController.Index), FromController = typeof(HomeController))]
    public IActionResult Index(int rowCount = 1, int pageNumber = 1) 
    { 
        var result = _pageHelper.GetPage(_repository.SprSetting, pageNumber, rowCount);
        return View(result);
    }
     
    [HttpGet]
    public async Task<IActionResult> Edit(int settingId) => PartialView(await GetSettingAsync(settingId));

    [HttpPost]
    public async Task Edit(SprSetting setting) => await _repository.Update(setting);

    private async Task<SprSetting?> GetSettingAsync(int id) => await _repository.SprSetting.Where(w => w.Id == id).SingleOrDefaultAsync(); 
}