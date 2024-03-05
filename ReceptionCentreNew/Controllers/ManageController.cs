using ReceptionCentreNew.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReceptionCentreNew.Areas.Identity.User;
using Microsoft.Owin.Security;

namespace ReceptionCentreNew.Controllers;
[Authorize]
public class ManageController : Controller
{
    private SignInManager<ApplicationUser> _signInManager;
    private UserManager<ApplicationUser> _userManager;

    public ManageController()
    {
    }

    public ManageController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        UserManager = userManager;
        SignInManager = signInManager;
    }

    public SignInManager<ApplicationUser> SignInManager
    {
        get
        {
            return _signInManager;
        }
        private set
        {
            _signInManager = value;
        }
    }

    public UserManager<ApplicationUser> UserManager
    {
        get
        {
            return _userManager;
        }
        private set
        {
            _userManager = value;
        }
    }

    //
    // GET: /Manage/Index
    //public async Task<IActionResult> Index(ManageMessageId? message)
    //{
    //    ViewBag.StatusMessage =
    //        message == ManageMessageId.ChangePasswordSuccess ? "Ваш пароль изменен."
    //        : message == ManageMessageId.SetPasswordSuccess ? "Пароль задан."
    //        : message == ManageMessageId.SetTwoFactorSuccess ? "Настроен поставщик двухфакторной проверки подлинности."
    //        : message == ManageMessageId.Error ? "Произошла ошибка."
    //        : message == ManageMessageId.AddPhoneSuccess ? "Ваш номер телефона добавлен."
    //        : message == ManageMessageId.RemovePhoneSuccess ? "Ваш номер телефона удален."
    //        : "";

    //    var user = await UserManager.GetUserAsync(User);
    //    var model = new IndexViewModel
    //    {
    //        HasPassword = HasPassword(),
    //        PhoneNumber = await UserManager.GetPhoneNumberAsync(user),
    //        TwoFactor = await UserManager.GetTwoFactorEnabledAsync(user),
    //        Logins = await UserManager.GetLoginsAsync(user),
    //        BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(user)
    //    };
    //    return View(model);
    //}

    //
    // POST: /Manage/RemoveLogin
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveLogin(string loginProvider, string providerKey)
    {
        ManageMessageId? message;
        var result = await UserManager.RemoveLoginAsync(await UserManager.GetUserAsync(User), loginProvider, providerKey);
        if (result.Succeeded)
        {
            var user = await UserManager.FindByIdAsync(UserManager.GetUserAsync(User).Id.ToString());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false);
            }
            message = ManageMessageId.RemoveLoginSuccess;
        }
        else
        {
            message = ManageMessageId.Error;
        }
        return RedirectToAction("ManageLogins", new { Message = message });
    }

    //
    // GET: /Manage/AddPhoneNumber
    public IActionResult AddPhoneNumber()
    {
        return View();
    }

    //
    // POST: /Manage/AddPhoneNumber
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return View(model);
    //    }
    //    // Создание и отправка маркера
    //    var code = await UserManager.GenerateChangePhoneNumberTokenAsync(await UserManager.GetUserAsync(User), model.Number);
    //    if (UserManager.SmsService != null)
    //    {
    //        var message = new IdentityMessage
    //        {
    //            Destination = model.Number,
    //            Body = "Ваш код безопасности: " + code
    //        };
    //        await UserManager.SmsService.SendAsync(message);
    //    }
    //    return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
    //}

    //
    // POST: /Manage/EnableTwoFactorAuthentication
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnableTwoFactorAuthentication()
    {
        await UserManager.SetTwoFactorEnabledAsync(await UserManager.GetUserAsync(User), true);
        var user = await UserManager.FindByIdAsync(UserManager.GetUserAsync(User).Id.ToString());
        if (user != null)
        {
            await SignInManager.SignInAsync(user, isPersistent: false);
        }
        return RedirectToAction("Index", "Manage");
    }

    //
    // POST: /Manage/DisableTwoFactorAuthentication
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DisableTwoFactorAuthentication()
    {
        await UserManager.SetTwoFactorEnabledAsync(await UserManager.GetUserAsync(User), false);
        var user = await UserManager.FindByIdAsync(UserManager.GetUserAsync(User).Id.ToString());
        if (user != null)
        {
            await SignInManager.SignInAsync(user, isPersistent: false);
        }
        return RedirectToAction("Index", "Manage");
    }

    //
    // GET: /Manage/VerifyPhoneNumber
    public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
    {
        var code = await UserManager.GenerateChangePhoneNumberTokenAsync(await UserManager.GetUserAsync(User), phoneNumber);
        // Отправка SMS через поставщик SMS для проверки номера телефона
        return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
    }

    //
    // POST: /Manage/VerifyPhoneNumber
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var result = await UserManager.ChangePhoneNumberAsync(await UserManager.GetUserAsync(User), model.PhoneNumber, model.Code);
        if (result.Succeeded)
        {
            var user = await UserManager.FindByIdAsync(UserManager.GetUserAsync(User).Id.ToString());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
        }
        // Это сообщение означает наличие ошибки; повторное отображение формы
        ModelState.AddModelError("", "Не удалось проверить телефон");
        return View(model);
    }

    //
    // POST: /Manage/RemovePhoneNumber
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemovePhoneNumber()
    {
        var result = await UserManager.SetPhoneNumberAsync(await UserManager.GetUserAsync(User), null);
        if (!result.Succeeded)
        {
            return RedirectToAction("Index", new { Message = ManageMessageId.Error });
        }
        var user = await UserManager.FindByIdAsync(UserManager.GetUserAsync(User).Id.ToString());
        if (user != null)
        {
            await SignInManager.SignInAsync(user, isPersistent: false);
        }
        return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
    }

    //
    // GET: /Manage/ChangePassword
    public IActionResult ChangePassword()
    {
        return View();
    }

    //
    // POST: /Manage/ChangePassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var result = await UserManager.ChangePasswordAsync(await UserManager.GetUserAsync(User), model.OldPassword, model.NewPassword);
        if (result.Succeeded)
        {
            var user = await UserManager.FindByIdAsync(UserManager.GetUserAsync(User).Id.ToString());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
        }
        //AddErrors(result);
        return View(model);
    }

    //
    // GET: /Manage/SetPassword
    public IActionResult SetPassword()
    {
        return View();
    }

    //
    // POST: /Manage/SetPassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await UserManager.AddPasswordAsync(await UserManager.GetUserAsync(User), model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(UserManager.GetUserAsync(User).Id.ToString());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
            }
            //  AddErrors(result);
        }

        // Это сообщение означает наличие ошибки; повторное отображение формы
        return View(model);
    }


    //GET: /Manage/ManageLogins
    public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
    {
        ViewData["StatusMessage"] =
            message == ManageMessageId.RemoveLoginSuccess ? "Внешнее имя входа удалено."
            : message == ManageMessageId.Error ? "Произошла ошибка."
            : "";

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return View("Error");
        }

        var userLogins = await _userManager.GetLoginsAsync(user);
        var otherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                            .Where(auth => userLogins.All(ul => auth.Name != ul.LoginProvider))
                            .ToList();

        ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;

        var model = new ManageLoginsViewModel
        {
            CurrentLogins = userLogins,
            OtherLogins = otherLogins as IList<AuthenticationDescription> ?? new List<AuthenticationDescription>(),
        };

        return View(model);
    }


    // GET: /Manage/LinkLoginCallback
    public async Task<IActionResult> LinkLoginCallback()
    {
        var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
        if (loginInfo == null)
        {
            return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        var user = await _userManager.GetUserAsync(User);
        var result = await _userManager.AddLoginAsync(user, loginInfo);

        return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && _userManager != null)
        {
            _userManager.Dispose();
            _userManager = null;
        }

        base.Dispose(disposing);
    }

    #region Вспомогательные приложения
    // Используется для защиты от XSRF-атак при добавлении внешних имен входа
    private const string XsrfKey = "XsrfId";


    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
    }

    private bool HasPassword()
    {
        var user = UserManager.GetUserAsync(User).Result;
        if (user != null)
        {
            return user.PasswordHash != null;
        }
        return false;
    }

    private bool HasPhoneNumber()
    {
        var user = UserManager.GetUserAsync(User).Result;
        return user != null ? user.PhoneNumber != null : false;
    }

    public enum ManageMessageId
    {
        AddPhoneSuccess,
        ChangePasswordSuccess,
        SetTwoFactorSuccess,
        SetPasswordSuccess,
        RemoveLoginSuccess,
        RemovePhoneSuccess,
        Error
    }

    #endregion
}