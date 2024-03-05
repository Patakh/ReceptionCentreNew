using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class ChartClaimInWeek
{
    [Column("out_date")]
    public DateOnly OutDate { get; set; }

    [Column("out_count_claim")]
    public int OutCountClaim {  get; set; }

    [Column("out_count_notify")]
    public int OutCountNotify {  get; set; }

    [Column("out_day_week")]
    public string OutDayWeek {  get; set; }
}