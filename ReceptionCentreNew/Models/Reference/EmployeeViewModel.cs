using ReceptionCentreNew.Data.Context.App;

namespace ReceptionCentreNew.Models;
public class EmployeeViewModel
{
    public IEnumerable<SprEmployees> SprEmployees { get; set; }
    public IEnumerable<SprEmployeesRoleJoin> SprEmployeesRoleJoin { get; set; }
    public IEnumerable<SprEmployeesDepartment> SprEmployeesDepartmentList { get; set; }
    public IEnumerable<SprEmployeesJobPos> SprEmployeesJobPosList { get; set; }
    public IEnumerable<SprEmployeesMessageTemplate> SprEmployeesMessageTemplateList { get; set; }
    public Guid EmployeeId { get; set; }
    public PageInfo PageInfo { get; set; }
}