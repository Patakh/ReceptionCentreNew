using Microsoft.AspNetCore.Mvc.Filters;
using ReceptionCentreNew.Data.Context.App;
using System.Net;
using System.Net.Mime;
using ReceptionCentreNew.Data.Context.App;

namespace ReceptionCentreNew.Filters;
public class ClientErrorHandler : Attribute, IExceptionFilter
{ 
    public void OnException(ExceptionContext context)
    {
        ReceptionCentreContext db = new();
        var response = context.HttpContext.Response;
        response.StatusCode = (int)HttpStatusCode.BadRequest;
        response.ContentType = MediaTypeNames.Text.Plain;

        var employeeName = context.HttpContext.User.Identity.Name;
        var employee = db.SprEmployees.FirstOrDefault(ss => ss.EmployeesLogin == employeeName);
        var employeeId = employee?.Id ?? Guid.Empty;

        var message = new DataSystemErrors
        {
            ErrorMessage = context.Exception.Message,
            ErrorInnerException = context.Exception.InnerException?.ToString(),
            EmployeesName = employee?.EmployeesName ?? "",
            StackTrace = context.Exception.StackTrace ?? "",
            SprEmployeesId = employeeId,
            ErrorDate = DateTime.Now
        };

        db.DataSystemErrors.Add(message);
        db.SaveChanges();

        context.ExceptionHandled = true;
    }
}