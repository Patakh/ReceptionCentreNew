using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class DataAppealClaimStatistics
{
    [Column("out_count_to_day")]
    public int OutCountToDay { get; set; }

    [Column("out_count_month")]
    public int OutCountMonth { get; set; }

    [Column("out_count_year")]
    public int OutCountYear { get; set; }
}