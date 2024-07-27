using Microsoft.AspNetCore.Mvc.Rendering;

namespace ReceptionCentreNew.Models.Appeal;

public class AppealsViewModel
{
    public SelectList Employees { get; set; }
    public SelectList Categories { get; set; }
    public SelectList Types { get; set; }
    public SelectList TypeDifficulties { get; set; }
    public SelectList Subjects { get; set; }
    public SelectList Statuses { get; set; }
    public Guid CurrentEmployeeId { get; set; }
}