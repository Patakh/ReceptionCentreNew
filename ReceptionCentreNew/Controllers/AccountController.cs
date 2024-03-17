using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceptionCentreNew.CustomCripto;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Data.Context.App.Abstract;
using ReceptionCentreNew.Models;
using System.Security.Claims; 

namespace ReceptionCentreNew.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    private IRepository _repository;
    public AccountController(IRepository repo, SignInManager<ApplicationUser> signInManager)
    {
        _repository = repo;
        _signInManager = signInManager;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LogOnModel model)
    {
        if (ModelState.IsValid)
        {  
            SprEmployees employee = await (from u in _repository.SprEmployees
                                           where u.EmployeesLogin == model.Name
                                           select u).FirstOrDefaultAsync();

            if (employee != null && employee.EmployeesPass != null && Crypto.VerifyHashedPassword(employee.EmployeesPass, model.Password))
            {

                List<Claim> claims = [
                    new Claim(ClaimTypes.Name, employee.EmployeesLogin),
                    new Claim(ClaimTypes.UserData, employee.EmployeesName),
                ];

                var roles = _repository.SprEmployeesRole.Where(s => s.Id == _repository.SprEmployeesRoleJoin.FirstOrDefault(f => f.SprEmployeesId == employee.Id).SprEmployeesRoleId);
                if (roles.Any())
                   await roles.ForEachAsync(role =>
                    {
                        claims.Add(new Claim(type: ClaimTypes.Role, value: role.RoleName));
                    });

                ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Неправильный пароль или логин");
            }
        }
        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }

    public IActionResult AccessDenied()
    {
        ViewBag.Title = "403 Запрещенный";
        return View();
    }
}