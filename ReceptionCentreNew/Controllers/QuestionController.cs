using ReceptionCentreNew.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Areas.Identity.User;
using ReceptionCentreNew.Data.Context.App.Abstract;

namespace ReceptionCentreNew.Controllers
{
    [ClientErrorHandler]
    [Authorize]
    public class QuestionController : Controller
    {
        #region Инициализация Repository
        private IRepository _repository;
        private string? UserName;
        public QuestionController(IRepository repo, UserManager<ApplicationUser> userManager)
        {
            _repository = repo;
            UserName = _repository.SprEmployees.First(s => s.Id == userManager.GetUserAsync(User).Result.EmployeeId.Value).EmployeesName;
        }
        #endregion
        // GET: Question
        public IActionResult Questions()
        {
            return View(_repository.SprQuestion.Where(w=>w.IsRemove!=true));
        }
    }
}