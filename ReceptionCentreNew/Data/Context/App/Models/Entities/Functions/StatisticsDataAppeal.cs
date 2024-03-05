using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class StatisticsDataAppeal
{  
    [Column("out_count_appeal")]
    public int OutCountAppeal { get; set; }

    [Column("out_month")]
    public int OutMonth { get; set; }

    [Column("out_year")]
    public int OutYear { get; set; }
}