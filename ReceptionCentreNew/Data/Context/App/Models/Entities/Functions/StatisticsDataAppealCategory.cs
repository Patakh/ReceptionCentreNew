using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class StatisticsDataAppealCategory
{
    [Column("out_count_appeal")]
    public int OutCountAppeal { get; set; }

    [Column("out_category_name")]
    public string? OutCategoryName { get; set; }
}