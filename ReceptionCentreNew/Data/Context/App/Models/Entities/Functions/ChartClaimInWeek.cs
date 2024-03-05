namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class ChartClaimInWeek
{
    public DateOnly OutDate { get; set; }
    public int OutCountClaim {  get; set; }
    public int OutCountNotify {  get; set; }
    public int OutDayWeek {  get; set; }
}