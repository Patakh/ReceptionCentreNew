using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace ReceptionCentreNew.Areas.Identity.User;
public class ApplicationUser : IdentityUser<Guid>
{
    [Column("spr_employees_department_id")]
    public Guid SprEmployeesDepartmentId { get; set; }

    [Column("spr_employees_job_pos_id")]
    public Guid SprEmployeesJobPosId { get; set; }

    [Column("employees_name")]
    public string EmployeesName { get; set; } = null!;

    [Column("employees_active")]
    public bool? EmployeesActive { get; set; }

    [Column("is_remove")]
    public bool IsRemove { get; set; }

    [Column("employees_name_add")]
    public string? EmployeesNameAdd { get; set; }

    [Column("date_add")]
    public DateTime? DateAdd { get; set; }

    [Column("employees_name_modify")]
    public string? EmployeesNameModify { get; set; }

    [Column("date_modify")]
    public DateTime? DateModify { get; set; }

    [Column("commentt_modify")]
    public string? CommenttModify { get; set; }

    [Column("can_take_appeal")]
    public bool CanTakeAppeal { get; set; }

    [Column("ip_address_add")]
    public string? IpAddressAdd { get; set; }

    [Column("ip_address_modify")]
    public string? IpAddressModify { get; set; }
}