using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class ReportCategory
{ 
    [Column("out_mfc_name")]
    public string? OutMfcName { get; set; }

    [Column("out_count_question")]
    public int OutCountQuestion { get; set; }

    [Column("out_num")]
    public int OutNum { get; set; }

    [Column("out_count_claim")]
    public int OutCountClaim { get; set; }

    [Column("out_count_review")]
    public int OutCountReview { get; set; }

    [Column("out_count_proposals")]
    public int OutCountProposals { get; set; }

    [Column("out_count_alert")]
    public int OutCountAlert { get; set; }
}