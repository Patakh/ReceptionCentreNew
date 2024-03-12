using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class ReportTreatment
{
    [Column("out_mfc_name")]
    public string? OutMfcName { get; set; }
      
    [Column("out_count_earth")]
    public int OutCountEarth { get; set; }

    [Column("out_count_social")]
    public int OutCountSocial { get; set; }

    [Column("out_count_migratory")]
    public int OutCountMigratory { get; set; }

    [Column("out_count_commercial")]
    public int OutCountCommercial { get; set; }

    [Column("out_count_quality")]
    public int OutCountQuality { get; set; }

    [Column("out_count_other")]
    public int OutCountOther { get; set; }

    [Column("out_num")]
    public int OutNum { get; set; }
}