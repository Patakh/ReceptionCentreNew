namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class DataAppealCallSelect
{
    public Guid OutId { get; set; }
    public DateTime? OutDateCall { get; set; }
    public DateTime? OutDateEmail { get; set; }
    public DateTime? OutTimeTalk { get; set; } 
    public string? OutPhoneNumber { get; set; }
    public string? OutNumberAppeal { get; set; }
    public string? OutEmployeesName { get; set; }
    public string? ApplicantName { get; set; }
    public string? OutCaption { get; set; }
    public int? OutCallType { get; set; } 
    public bool? OutSaveFtp { get; set; }
}