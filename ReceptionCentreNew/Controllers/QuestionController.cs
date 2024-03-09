using ReceptionCentreNew.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Data.Context.App.Abstract;
using ReceptionCentreNew.Models;

namespace ReceptionCentreNew.Controllers
{
    [ClientErrorHandler]
    [Authorize]
    public class QuestionController : Controller
    {
        #region Инициализация Repository
        private IRepository _repository;
        private string? UserName;
        public QuestionController(IRepository repo, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _repository = repo;
            UserName = _repository.SprEmployees.First(s => s.EmployeesLogin == signInManager.Context.User.Identity.Name).EmployeesName;
        }
        #endregion
        // GET: Question
        public IActionResult Questions()
        {
            return View(_repository.SprQuestion.Where(w=>w.IsRemove!=true));
        }
    }
}