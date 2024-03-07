using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class DataAppealClaimWeek
{
    [Column("out_date")]
    public DateTime OutDate { get; set; }

    [Column("out_count_claim")]
    public int OutCountClaim { get; set; }
}
