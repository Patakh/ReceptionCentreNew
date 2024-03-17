using ReceptionCentreNew.Models.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using ReceptionCentreNew.Data.Context.App.Abstract;
using ReceptionCentreNew.Data.Context.App;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Models;
using SmartBreadcrumbs.Attributes;

namespace ReceptionCentreNew.Controllers;
[Authorize] 
public class HomeController : Controller
{
    public int PageSize = 10;
    private IRepository _repository;
    public SignInManager<ApplicationUser> SignInManager;
    public HomeController(IRepository repo, SignInManager<ApplicationUser> signInManager)
    {
        SignInManager = signInManager;
        _repository = repo;
    }

    [DefaultBreadcrumb("Главная")]
    public IActionResult Index(string PhoneNumber)
    {
        ViewBag.IncomingCollFromJitsi = PhoneNumber;
        return View();
    }

    public IActionResult About()
    {
        ViewBag.Message = "Your application description page.";

        return View();
    }

    public IActionResult Contact()
    {
        ViewBag.Message = "Your contact page.";

        return View();
    }

    public enum SearchType
    {
        SearchCustomer = 1,
        SearchServiceCustomer = 2
    }

    public IActionResult Search(string search = "")
    {
        var searchTextArray = search.ToUpper().Split();
        if (searchTextArray.Length > 0)
        {
            object result = null;
            result = searchTextArray.Aggregate(_repository.DataAppeal.Include(i => i.SprStatus), (current, searchTextItem) => (IIncludableQueryable<DataAppeal, SprStatus>)current.Where(
                r => r.NumberAppeal.ToUpper().Contains(searchTextItem)
                || r.EmployeesNameAdd.ToUpper().Contains(searchTextItem)
                || r.TextAppeal.ToUpper().Contains(searchTextItem)
                || r.PhoneNumber.ToUpper().Contains(searchTextItem)
                || r.ApplicantName.ToUpper().Contains(searchTextItem)))
                .Select(s => new { TextAppeal = s.TextAppeal.Length > 150 ? s.TextAppeal.Substring(0, 150) + "..." : s.TextAppeal, s.NumberAppeal, s.EmployeesNameAdd, s.DateAdd, s.ApplicantName, s.SprStatus.StatusName }).OrderByDescending(o => o.DateAdd).ToList();
            return Json(result);
        }

        return Json(null);
    }
    public string GetIncomingCallData(JitsiRequest request)
    {
        var employee = _repository.SprEmployees.SingleOrDefault(se => se.EmployeesLogin == SignInManager.Context.User.Identity.Name);
        Guid? mfc_id = employee.SprEmployeesDepartmentId;
        Guid? employees_id = employee.Id;
        string EmployeeFio = employee.EmployeesName;
        request.IdMfc = mfc_id?.ToString();
        request.IdEmployee = employees_id?.ToString();
        request.EmployeeFio = EmployeeFio;
        request.Command = 0;
        return Encoding.Default.GetString(CRPassword.Jitsi_encode(JsonConvert.SerializeObject(request)));
    }
}