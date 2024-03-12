using ReceptionCentreNew.Models;
using ReceptionCentreNew.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReceptionCentreNew.Data.Context.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Data.Context.App.Abstract;

namespace ReceptionCentreNew.Controllers
{
    [ClientErrorHandler]
    [Authorize]
    public class NotificationController : Controller
    {
        public int PageSize = 10;
         
        private IRepository _repository;
        private string? UserName;
        public SignInManager<ApplicationUser> SignInManager;
        public NotificationController(IRepository repo, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _repository = repo;
            SignInManager = signInManager;
            UserName = _repository.SprEmployees.First(s => s.EmployeesLogin == signInManager.Context.User.Identity.Name).EmployeesName;
        } 
        
        public IActionResult Notifications()
        {
            var employees = _repository.SprEmployees.Where(e => e.IsRemove != true);
            if (!User.IsInRole("superadmin") && !User.IsInRole("admin"))
            {
                employees = employees.Where(se => se.EmployeesLogin ==SignInManager.Context.User.Identity.Name);
            }
            var notify = _repository.SprNotification;
            ViewBag.SprTypeNotification = new SelectList(notify, "Id", "Notification");
            return View();
        }

        public IActionResult PartialTableNotifications(short? period, short? sprTypeNotificationId, string search, bool? IsActive, int page = 1)
        {
            ViewBag.SearchNotify = search;
            ViewBag.Period = period;
            ViewBag.TypeId = sprTypeNotificationId;
            ViewBag.IsActive = IsActive;

            Guid SprEmployeeId = _repository.SprEmployees.Where(w=>w.EmployeesLogin== SignInManager.Context.User.Identity.Name).SingleOrDefault().Id;
            
            DateTime dateStart;
            switch (period)
            {
                case 1: dateStart = DateTime.Now.Date; break;
                case 2: dateStart = DateTime.Now.AddDays(-7).Date; break;
                case 3: dateStart = DateTime.Now.AddMonths(-1).Date; break;
                case 4: dateStart = DateTime.Now.AddYears(-1).Date; break;
                default: dateStart = DateTime.Now.AddYears(-2).Date; break;
            }
            DateTime dateStop = DateTime.Now.AddDays(1).Date;
            IEnumerable<DataEmployeesNotification> notifications = _repository.DataEmployeesNotification.Include(i=>i.SprNotification).Include(i=>i.DataAppeal.SprMfc).Where(w=>w.SprEmployeesId==SprEmployeeId && w.DateReceipt >= dateStart && w.DateReceipt <= dateStop);
            if(sprTypeNotificationId != null)
            {
                notifications = notifications.Where(w => w.SprNotificationId == sprTypeNotificationId);
            }
            if(IsActive != null)
            {
                notifications = notifications.Where(w => w.IsActive == IsActive);
            }            
            notifications = String.IsNullOrEmpty(search) ? notifications :
                search.ToLower().Split().Aggregate(notifications, (current, item) => current.Where(h => h.DataAppeal.ApplicantName.ToLower().Contains(item) || h.DataAppeal.NumberAppeal.ToLower().Contains(item) || h.SprNotification.Notification.ToLower().Contains(item) || h.DateReceipt.ToShortDateString().Contains(item)));

            NotificationsViewModel model = new()
            {
                NotificationList = notifications.OrderByDescending(o=>o.DateReceipt).Skip((page - 1) * PageSize).Take(PageSize),
                PageInfo = new PageInfo
                {
                    MaxPageList = 5,
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = notifications.Count()
                },
            };
            return PartialView(model);
        }

        public IActionResult SubmitNoficationSave(Guid notifyId)
        {
            DataEmployeesNotification notify = _repository.DataEmployeesNotification.Where(w => w.Id == notifyId).FirstOrDefault();
            notify.IsActive = notify.IsActive == true ? false : true;
            _repository.Update(notify);
            return Json("OK");
        }
    }
}