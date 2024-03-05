namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class ChartInWeek
{
    public DateOnly OutDate { get; set; }
    public int OutCountIncoming { get; set; }
    public int OutCountOutgoing { get; set; }
    public int OutDayWeek { get; set; } 
}