using ReceptionCentreNew.Domain.Models.Entities.Functions;

namespace ReceptionCentreNew.Models;
public class ReportParameters
{
    public Guid? SprEmployeeId { get; set; }
    public int? SprStatusId { get; set; }
    public Guid? SprTypeId { get; set; }
    public Guid? SprTypeDifficultyId { get; set; }
    public Guid? SprSubjectId { get; set; }
    public Guid? SprCategoryId { get; set; }
    public int? Period { get; set; }
    public short? Type { get; set; }
    public short? IsConnected { get; set; }
}

public class ReportViewModel
{
    public IEnumerable<ReportCategory> ReportCategoryList { get; set; }
    public IEnumerable<ReportTreatment> ReportTreatmentList { get; set; }
}