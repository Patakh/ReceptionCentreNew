using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class StatisticsDataAppealCall
{
    [Column("out_month")]
    public int OutMonth { get; set; }

    [Column("out_count_call_incoming")]
    public int OutCountCallIncoming { get; set; }

    [Column("out_count_call_outgoing")]
    public int OutCountCallOutgoing { get; set; }

    [Column("out_count_call_missed")]
    public int OutCountCallMissed { get; set; }

    [Column("out_year")]
    public int OutYear { get; set; }
}