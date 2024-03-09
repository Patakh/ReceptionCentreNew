using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Models;
using System.Security.Claims;
using System.Web.Helpers;

namespace ReceptionCentreNew.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    public AccountController(SignInManager<ApplicationUser> signInManager)
    {
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
            ReceptionCentreContext context = new();

            SprEmployees employee = await (from u in context.SprEmployees
                                           where u.EmployeesLogin == model.Name
                                           select u).FirstOrDefaultAsync();

            if (employee != null && Crypto.VerifyHashedPassword(employee.EmployeesPass, model.Password))
            {
                var role = context.SprEmployeesRole.First(s => s.Id == context.SprEmployeesRoleJoin.First(f => f.SprEmployeesId == employee.Id).SprEmployeesRoleId).RoleName;
                List<Claim> claims = [
                    new Claim(ClaimTypes.Name, employee.EmployeesLogin),
                    new Claim(ClaimTypes.UserData, employee.EmployeesName),
                    new Claim(ClaimTypes.Role, role), 
                ]; 


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
}