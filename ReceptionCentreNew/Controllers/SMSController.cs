﻿using ReceptionCentreNew.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Data.Context.App.Abstract;

namespace ReceptionCentreNew.Controllers;
[Authorize]
public class SMSController : Controller
{
    private IRepository _repository;
    private string? UserName;
    public SignInManager<ApplicationUser> SignInManager;
    public SMSController(IRepository repo, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        SignInManager = signInManager;
        _repository = repo;
        UserName = _repository.SprEmployees.First(s => s.EmployeesLogin == signInManager.Context.User.Identity.Name).EmployeesName;
    }

    // GET: SMS
    public IActionResult Sms()
    {
        var employees = _repository.SprEmployees.Where(e => e.IsRemove != true);

        if (!User.IsInRole("superadmin") && !User.IsInRole("admin"))
            employees = employees.Where(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);

        ViewBag.SprEmployees = new SelectList(employees, "Id", "EmployeesName");
        return View();
    }
    public IActionResult PartialTableSms(Guid? SprEmployeeId, short? period, short? type)
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
        var sms = _repository.DataAppealMessage;
        int type_query = 0;
        if (SprEmployeeId.HasValue && type.HasValue)
        { type_query = 1; }
        else if (SprEmployeeId.HasValue && type == null)
        { type_query = 2; }
        else if (SprEmployeeId == null && type.HasValue)
        { type_query = 3; }
        else { type_query = 4; }
        switch (type_query)
        {
            case 1: sms = sms.Where(w => w.SprEmployeesId == SprEmployeeId && w.DateMessage >= dateStart && w.MessageType == type); break;
            case 2: sms = sms.Where(w => w.SprEmployeesId == SprEmployeeId && w.DateMessage >= dateStart); break;
            case 3: sms = sms.Where(w => w.DateMessage >= dateStart && w.MessageType == type); break;
            case 4: sms = sms.Where(w => w.DateMessage >= dateStart); break;
        }
        SmsViewModel model = new()
        {
            SmsList = sms,
        };
        return PartialView(model);
    }
}