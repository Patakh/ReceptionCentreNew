using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class DataAppealCallSelect
{
    [Column("out_id")]
    public Guid OutId { get; set; }

    [Column("out_date_call")]
    public DateTime OutDateCall { get; set; }
      
    [Column("out_time_talk")]
    public string? OutTimeTalk { get; set; }

    [Column("out_phone_number")]
    public string? OutPhoneNumber { get; set; }

    [Column("out_number_appeal")]
    public string? OutNumberAppeal { get; set; }

    [Column("out_employees_name")]
    public string? OutEmployeesName { get; set; }

    [NotMapped]
    public string? ApplicantName { get; set; }
     
    [Column("out_call_type")]
    public int? OutCallType { get; set; }

    [Column("out_save_ftp")]
    public bool OutSaveFtp { get; set; }
}