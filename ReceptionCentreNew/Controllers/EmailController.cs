using ReceptionCentreNew.Models;
using ReceptionCentreNew.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReceptionCentreNew.Areas.Identity.User;
using ReceptionCentreNew.Data.Context.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Data.Context.App.Abstract;

namespace ReceptionCentreNew.Controllers
{
    [ClientErrorHandler]
    [Authorize]
    public class EmailController : Controller
    {
        public int PageSize = 10;

        #region Инициализация Repository
        private IRepository _repository;
        public EmailController(IRepository repo)
        {
            _repository = repo;
        }
        #endregion
        // GET: Email
        public IActionResult Emails()
        {
            var employees = _repository.SprEmployees.Where(e => e.IsRemove != true);
            if (!User.IsInRole("superadmin") && !User.IsInRole("admin"))
            {
                employees = employees.Where(se => se.EmployeesLogin == User.Identity.Name);
            }
            ViewBag.SprEmployees = new SelectList(employees, "id", "EmployeesName");            
            return View();
        }
        public IActionResult PartialTableEmails(Guid? SprEmployeeId, short? period, short? type, short? IsConnected, string search, int page = 1)
        {
            ViewBag.SprEmployeeId = SprEmployeeId;
            ViewBag.Period = period;
            ViewBag.Type = type;
            ViewBag.IsConnected = IsConnected;
            ViewBag.SearchEmail = search;
            DateTime dateStart;
            switch (period)
            {
                case 1: dateStart = DateTime.Now; break;
                case 2: dateStart = DateTime.Now.AddDays(-7); break;
                case 3: dateStart = DateTime.Now.AddMonths(-1); break;
                case 4: dateStart = DateTime.Now.AddYears(-1); break;
                default: dateStart = DateTime.Now.AddYears(-2); break;
            }
            var emails = _repository.FuncDataAppealEmailSelect(SprEmployeeId, dateStart, DateTime.Now.AddDays(1), type, IsConnected);
            if(!String.IsNullOrEmpty(search))
            {
                emails = search.ToLower().Split().Aggregate(emails, (current, item) => current.Where(h => h.OutTextEmail.ToLower().Contains(item) || h.OutEmployeesName.ToLower().Contains(item)));
            }
            
            EmailsViewModel model = new EmailsViewModel
            {
                EmailList = emails.OrderByDescending(o => o.OutDateEmail).Skip((page - 1) * PageSize).Take(PageSize),
                PageInfo = new PageInfo
                {
                    MaxPageList = 5,
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = emails.Count()
                },
            };
            return PartialView(model);
        }
        public IActionResult PartialModalReadEmail(Guid emailId)
        {
            DataAppealEmail model = _repository.DataAppealEmail.Include(i=>i.DataAppeal).Where(w=>w.Id==emailId).SingleOrDefault();
            model.IsRead = true;
            _repository.Update(model);            
            return PartialView("PartialModalReadEmail", model);
        }

        public IActionResult SubmitEmailSave(Guid emailId)
        {
            DataAppealEmail email = _repository.DataAppealEmail.Where(w => w.Id == emailId).FirstOrDefault();
            email.IsRead = email.IsRead == true ? false : true;
            _repository.Update(email);
            return Json("OK");
        }
    }
}