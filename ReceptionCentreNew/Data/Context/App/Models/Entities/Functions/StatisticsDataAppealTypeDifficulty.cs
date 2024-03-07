using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class StatisticsDataAppealTypeDifficulty
{
    [Column("out_count_appeal")]
    public int OutCountAppeal { get; set; }

    [Column("out_type_name")]
    public string? OutTypeName { get; set; }
}