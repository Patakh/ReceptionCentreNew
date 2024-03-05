using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;

public class ChartClaimForMfc
{
    [Column("out_mfc_name")]
    public string OutMfcName { get; set; }

    [Column("out_count_claim")]
    public int OutCountClaim { get; set; }

    [Column("out_count_notify")]
    public int OutCountNotify { get; set; } 
}