using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class DataAppealClaimYear
{
    [Column("out_count_claim")]
    public int OuCounClaim { get; set; }

    [Column("out_month")]
    public string OuMonth { get; set; }

    [Column("out_year")]
    public string OutYear { get; set; }
}