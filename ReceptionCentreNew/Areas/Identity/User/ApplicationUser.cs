using Microsoft.AspNetCore.Identity; 
namespace ReceptionCentreNew.Areas.Identity.User;
public class ApplicationUser : IdentityUser<Guid>
{
    public Guid? EmployeeId { get; set; } 
}