using Microsoft.AspNetCore.Identity;
namespace ReceptionCentreNew.Areas.Identity.Role;
public class EmployeesRole : IdentityRole<Guid>
{
    public EmployeesRole()
    {

    }

    public EmployeesRole(string roleName) : base(roleName)
    {
        Name = roleName;
    }

    public string? Description { get; set; }
}