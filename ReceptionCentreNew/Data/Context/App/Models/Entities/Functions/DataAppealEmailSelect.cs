namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class DataAppealEmailSelect
{
    public Guid OutId { get; set; }
    public string? OutEmail { get; set; }
    public string? OutCaption { get; set; }
    public string? OutEmployeesName { get; set; }
    public string? OutNumberAppeal { get; set; }
    public DateTime? OutDateEmail { get; set; }
}