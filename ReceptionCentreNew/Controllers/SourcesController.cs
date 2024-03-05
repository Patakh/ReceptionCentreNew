using ReceptionCentreNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReceptionCentreNew.Areas.Identity.User;
using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Data.Context.App.Abstract;

namespace ReceptionCentreNew.Controllers
{
    public class SourcesController : Controller
    {
        public int PageSize = 10;
        #region Инициализация Repository
        private IRepository _repository;
        private string? UserName;
        public SourcesController(IRepository repo, UserManager<ApplicationUser> userManager)
        {
            _repository = repo;
            UserName = _repository.SprEmployees.First(s => s.Id == userManager.GetUserAsync(User).Result.EmployeeId.Value).EmployeesName;
        }
        #endregion
        // GET: Sousces
        public IActionResult In()
        {
            return View();
        }
        public IActionResult PartialTableSources(short? action_, Guid? SprEmployeeId, short? period, short? IsConnected, short? sources, string search, int page = 1)
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

            IEnumerable<SourcesModel> m = new List<SourcesModel>();
            switch (sources)
            {
                case 1:
                    var calls = _repository.FuncDataAppealCallSelect(SprEmployeeId, dateStart, DateTime.Now.AddDays(1), action_, IsConnected).ToArray();
                    m = calls.Select(s => new SourcesModel { Id = s.OutId, NumberAppeal = s.OutNumberAppeal, EmployeesName = s.OutEmployeesName, ApplicantName = s.ApplicantName, contact = s.OutPhoneNumber, DateAdd = s.OutDateCall, Option = s.OutTimeTalk.ToString(), SaveFtp = s.OutSaveFtp.Value, type = SourcesType.call }); break;
                case 2:
                    var emails = _repository.FuncDataAppealEmailSelect(SprEmployeeId, dateStart, DateTime.Now.AddDays(1), action_, IsConnected).ToArray();
                    m = emails.Select(s => new SourcesModel { Id = s.OutId, NumberAppeal = s.OutNumberAppeal, EmployeesName = s.OutEmployeesName, ApplicantName = null, contact = s.OutEmail, DateAdd = s.OutDateEmail, Option = s.OutCaption, SaveFtp = false, type = SourcesType.email });break;
                case 3: break;
                case 4: break;
                default:
                    calls = _repository.FuncDataAppealCallSelect(SprEmployeeId, dateStart, DateTime.Now.AddDays(1), action_, IsConnected).ToArray();
                    var calls_model = calls.Select(s => new SourcesModel { Id = s.OutId, NumberAppeal = s.OutNumberAppeal, EmployeesName = s.OutEmployeesName, ApplicantName = s.ApplicantName, contact = s.OutPhoneNumber, DateAdd = s.OutDateCall, Option = s.OutCaption, SaveFtp = s.OutSaveFtp.Value, type = SourcesType.call });
                    emails = _repository.FuncDataAppealEmailSelect(SprEmployeeId, dateStart, DateTime.Now.AddDays(1), action_, IsConnected).ToArray();
                    var emails_model = emails.Select(s => new SourcesModel { Id = s.OutId, NumberAppeal = s.OutNumberAppeal, EmployeesName = s.OutEmployeesName, ApplicantName = null, contact = s.OutEmail, DateAdd = s.OutDateEmail, Option = s.OutCaption, SaveFtp = false, type = SourcesType.email });
                    m = calls_model.Concat(emails_model);break;
            }
           
            m = String.IsNullOrEmpty(search) ? m :
                search.ToLower().Split().Aggregate(m, (current, item) => current.Where(h => 
                (h.ApplicantName != null ? h.ApplicantName.ToLower().Contains(item) : false)
                || (h.contact != null ? h.contact.ToLower().Contains(item) : false)
                || (h.DateAdd != null ? h.DateAdd.ToString().ToLower().Contains(item) : false) 
                || (h.NumberAppeal != null ? h.NumberAppeal.ToLower().Contains(item) : false)
                || (h.Option != null ? h.Option.ToLower().Contains(item) : false)
                || (h.EmployeesName != null ? h.EmployeesName.ToLower().Contains(item) : false)
                ));

            var getData = m.OrderByDescending(o => o.DateAdd).Skip((page - 1) * PageSize).Take(PageSize);
            List<SourcesModel> Sources = new List<SourcesModel>();
            foreach (var item in getData)
            {
                string phone = item.contact;
                if (phone.Length == 11)
                {
                    phone = "+7(" + phone.Substring(1, 3) + ")" + phone.Substring(4, 3) + "-" + phone.Substring(7, 2) + "-" + phone.Substring(9, 2);
                }
                var appeal = _repository.DataAppeal.Where(w => (w.PhoneNumber == phone || w.PhoneNumber == item.contact) && w.ApplicantName.Length > 3).FirstOrDefault();
                item.ApplicantName = appeal != null ? appeal.ApplicantName : "Заявитель не найден!";
                Sources.Add(item);
            }
            SourcesViewModel model = new SourcesViewModel()
            {
                SourceModel = Sources,
                PageInfo = new PageInfo
                {
                    MaxPageList = 5,
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = m.Count()
                },
            };
            ViewBag.Action = action_;
            ViewBag.Sources = sources;
            ViewBag.SprEmployeesId = SprEmployeeId;
            ViewBag.Search = search;
            ViewBag.Period = period;
            ViewBag.IsConnected = IsConnected;

            return PartialView(model);
        }
        public IActionResult Out()
        {
            var employees = _repository.SprEmployees.Where(e => e.IsRemove != true);
            if (!User.IsInRole("superadmin") && !User.IsInRole("admin"))
            {
                employees = employees.Where(se => se.EmployeesLogin == User.Identity.Name);
            }
            ViewBag.SprEmployees = new SelectList(employees, "Id", "EmployeesName");
            return View();
        }

    }
}