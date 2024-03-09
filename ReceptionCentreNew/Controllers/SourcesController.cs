using ReceptionCentreNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Data.Context.App.Abstract;
using ReceptionCentreNew.Data.Context.App;
namespace ReceptionCentreNew.Controllers;
public class SourcesController : Controller
{
    public int PageSize = 10;
    private IRepository _repository;
    private SprEmployees _employee;
    public SourcesController(IRepository repo, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _repository = repo;
        _employee = _repository.SprEmployees.First(s => s.EmployeesLogin == signInManager.Context.User.Identity.Name);
    }
    public IActionResult In()
    {
        return View();
    }
    public ActionResult PartialTableSources(short? action_, Guid? sprEmployeeId, short? period, short? isConnected, short? sources, string search, int page = 1)
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
                var calls = _repository.FuncDataAppealCallSelect(sprEmployeeId, dateStart, DateTime.Now.AddDays(1), action_, isConnected).ToArray();
                m = calls.Select(s => new SourcesModel { Id = s.OutId, NumberAppeal = s.OutNumberAppeal, EmployeesName = s.OutEmployeesName, ApplicantName = s.ApplicantName, Contact = s.OutPhoneNumber, DateAdd = s.OutDateCall, Option = s.OutTimeTalk, SaveFtp = s.OutSaveFtp, Type = SourcesType.call }); break;
            case 2:
                var emails = _repository.FuncDataAppealEmailSelect(sprEmployeeId, dateStart, DateTime.Now.AddDays(1), action_, isConnected).ToArray();
                m = emails.Select(s => new SourcesModel { Id = s.OutId, NumberAppeal = s.OutNumberAppeal, EmployeesName = s.OutEmployeesName, ApplicantName = null, Contact = s.OutTextEmail, DateAdd = s.OutDateEmail, Option = s.OutCaption, SaveFtp = false, Type = SourcesType.email }); break;
            case 3: break;
            case 4: break;
            default:
                calls = _repository.FuncDataAppealCallSelect(sprEmployeeId, dateStart, DateTime.Now.AddDays(1), action_, isConnected).ToArray();
                var calls_model = calls.Select(s => new SourcesModel { Id = s.OutId, NumberAppeal = s.OutNumberAppeal, EmployeesName = s.OutEmployeesName, Contact = s.OutPhoneNumber, DateAdd = s.OutDateCall, SaveFtp = s.OutSaveFtp, Type = SourcesType.call });
                emails = _repository.FuncDataAppealEmailSelect(sprEmployeeId, dateStart, DateTime.Now.AddDays(1), action_, isConnected).ToArray();
                var emails_model = emails.Select(s => new SourcesModel { Id = s.OutId, NumberAppeal = s.OutNumberAppeal, EmployeesName = s.OutEmployeesName, ApplicantName = null, Contact = s.OutTextEmail, DateAdd = s.OutDateEmail, Option = s.OutCaption, SaveFtp = false, Type = SourcesType.email });
                m = calls_model.Concat(emails_model); break;
        }

        m = String.IsNullOrEmpty(search) ? m :
            search.ToLower().Split().Aggregate(m, (current, item) => current.Where(h =>
            (h.ApplicantName != null ? h.ApplicantName.ToLower().Contains(item) : false)
            || (h.Contact != null ? h.Contact.ToLower().Contains(item) : false)
            || (h.DateAdd != null ? h.DateAdd.ToString().ToLower().Contains(item) : false)
            || (h.NumberAppeal != null ? h.NumberAppeal.ToLower().Contains(item) : false)
            || (h.Option != null ? h.Option.ToLower().Contains(item) : false)
            || (h.EmployeesName != null ? h.EmployeesName.ToLower().Contains(item) : false)
            ));

        var getData = m.OrderByDescending(o => o.DateAdd).Skip((page - 1) * PageSize).Take(PageSize);
        List<SourcesModel> Sources = new();
        foreach (var item in getData)
        {
            string phone = item.Contact;
            if (phone.Length == 11)
            {
                phone = "+7(" + phone.Substring(1, 3) + ")" + phone.Substring(4, 3) + "-" + phone.Substring(7, 2) + "-" + phone.Substring(9, 2);
            }
            var appeal = _repository.DataAppeal.Where(w => (w.PhoneNumber == phone || w.PhoneNumber == item.Contact) && w.ApplicantName.Length > 3).FirstOrDefault();
            item.ApplicantName = appeal != null ? appeal.ApplicantName : "Заявитель не найден!";
            Sources.Add(item);
        }
        SourcesViewModel model = new()
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
        ViewBag.SprEmployeesId = action_ == 1 ? _employee.Id : sprEmployeeId;
        ViewBag.Search = search;
        ViewBag.Period = period;
        ViewBag.isConnected = isConnected;

        return PartialView(model);
    }
    public IActionResult Out()
    {
        var employees = _repository.SprEmployees.Where(e => e.IsRemove != true);
        if (!User.IsInRole("superadmin") && !User.IsInRole("admin"))
            employees = employees.Where(se => se.EmployeesLogin == User.Identity.Name);

        ViewBag.SprEmployees = new SelectList(employees, "Id", "EmployeesName");
        return View();
    }
}