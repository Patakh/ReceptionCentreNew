using ReceptionCentreNew.Filters;
using ReceptionCentreNew.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceptionCentreNew.Data.Context.App.Abstract;

namespace ReceptionCentreNew.Controllers
{
    [ClientErrorHandler]
    [Authorize]
    public class CallBackController : Controller
    {
        private IRepository _repository;
        public CallBackController(IRepository repo)
        {
             _repository = repo;
        }
        // GET: CallBack
        public IActionResult Index()
        {
            return View();
        }
        //[AllowAnonymous]
        //public void signalRCallback(Guid Id)
        //{
        //    var login = _repository.SprEmployees.First(q => q.Id == Id).EmployeesLogin;
        //    var context = new NotificationHub();
        //    context.Clients.User(login).callback(true);
        //}
        /// <summary>
        /// Все заказанные звонки
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCallBackCount()
        {
            var date_ = DateTime.Now.AddMonths(-3);
            var model = _repository.DataCallback.Where(w => w.Status == 1 && w.DateOrder >= date_).Count();
            return Json(model);
        }
        /// <summary>
        /// Список звонков
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCallBackView()
        {
            var dataa = _repository.DataAppeal.FirstOrDefault();
            var model = _repository.DataCallback.Include(i=>i.DataCallbackCalls).Where(w => w.Status == 1).OrderBy(o => o.DateOrder);
            return PartialView("_Table", model);
        }
        public IActionResult CallBackClose(Guid Id)
        {
            var model = _repository.DataCallback.FirstOrDefault(f => f.Id == Id);
            try
            {
                model.DateClose =new DateOnly(DateTime.Now.Year, DateTime.Now.Month,DateTime.Now.Day);
                model.SprEmployeesIdClose = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == User.Identity.Name)?.Id;
                model.Status = 4;
                model.IsHand = true;
                _repository.Update(model);
                return Json(model.Id);
            }
            catch
            {
                return null;
            }
        }
    }
}