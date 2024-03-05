using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class ChartInWeek
{
    [Column("out_date")]
    public DateOnly OutDate { get; set; }

    [Column("out_count_incoming")]
    public int OutCountIncoming { get; set; }

    [Column("out_count_outgoing")]
    public int OutCountOutgoing { get; set; }

    [Column("out_day_week")]
    public string OutDayWeek { get; set; } 
}