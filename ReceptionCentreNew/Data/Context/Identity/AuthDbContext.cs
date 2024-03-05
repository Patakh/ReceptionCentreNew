using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReceptionCentreNew.Areas.Identity.Role;
using ReceptionCentreNew.Areas.Identity.User; 

namespace ReceptionCentreNew.Data.Context.Identity;
public class AuthenticationContext : IdentityDbContext<ApplicationUser, EmployeesRole, Guid>
{
    public AuthenticationContext(DbContextOptions<AuthenticationContext> options)
        : base(options)
    { 
    }
    public AuthenticationContext()
    { 
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Server=194.87.144.157;User Id=postgres;Password=!ShamiL19;Port=5432;Database=Reception_centre;CommandTimeout=1000;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
         base.OnModelCreating(modelBuilder);
    }
}