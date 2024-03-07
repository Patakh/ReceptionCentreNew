using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class DataAppealEmailSelect
{
    [Column("out_id")]
    public Guid OutId { get; set; }

    [Column("out_text_email")]
    public string? OutTextEmail { get; set; }

    [Column("out_email_")]
    public string? OutEmail { get; set; }

    [Column("out_email_type")]
    public int OutEmailType { get; set; }

    [Column("out_employees_name")]
    public string? OutEmployeesName { get; set; }

    [Column("out_number_appeal")]
    public string? OutNumberAppeal { get; set; }

    [Column("out_uids_")]
    public string OutUids { get; set; }

    [Column("out_is_read")]
    public bool OutIsRead { get; set; }

    [Column("out_caption")]
    public string OutCaption { get; set; }

    [Column("out_date_email")]
    public DateTime? OutDateEmail { get; set; }
}