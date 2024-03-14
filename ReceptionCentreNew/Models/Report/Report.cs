using ReceptionCentreNew.Domain.Models.Entities.Functions;

namespace ReceptionCentreNew.Models;
public class ReportParameters
{
    private DateTime _dateStart;
    private DateTime _dateStop;

    public Guid? SprEmployeeId { get; set; }
    public int? SprStatusId { get; set; }
    public Guid? SprTypeId { get; set; }
    public Guid? SprTypeDifficultyId { get; set; }
    public Guid? SprSubjectId { get; set; }
    public Guid? SprCategoryId { get; set; }

    public DateTime DateStart
    {
        get =>
        _dateStart == DateTime.MinValue
                && !string.IsNullOrEmpty(Period)
                && Period.Contains('-')
                && DateTime.TryParse(Period.Split(' ')[0], out _dateStart)
            ? _dateStart
            : _dateStart;

        set =>
            _dateStart = value;
    }
    public DateTime DateStop
    {
        get =>
        _dateStop == DateTime.MinValue
              && !string.IsNullOrEmpty(Period)
              && Period.Contains('-')
              && DateTime.TryParse(Period.Split(' ')[2], out _dateStop)
           ? _dateStop
           : _dateStop;

        set =>
            _dateStop = value;
    }

    public short? Type { get; set; }
    public string Period { get; set; }

    public short? IsConnected { get; set; }
}

public class ReportViewModel
{
    public IEnumerable<ReportCategory> ReportCategoryList { get; set; }
    public IEnumerable<ReportTreatment> ReportTreatmentList { get; set; }
}