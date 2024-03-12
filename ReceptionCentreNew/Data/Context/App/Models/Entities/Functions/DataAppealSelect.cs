namespace ReceptionCentreNew.Data.Context.App;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class DataAppealSelect
{
    [Column("out_data_appeal_id")]
    public Guid OutDataAppealId { get; set; }

    [NotMapped] 
    public Guid? OutSprEmployeesId { get; set; }
     
    [Column("out_number_appeal")]
    public string? OutNumberAppeal { get; set; }
     
    [Column("out_employees_name")]
    public string? OutEmployeesName { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy HH':'mm}", ApplyFormatInEditMode = true)]

    [Column("out_date_add")]
    public DateTime? OutDateAdd { get; set; }
      
    [Column("out_applicant_name")]
    public string? OutApplicantName { get; set; }

    [Column("out_count_day")]
    public int? OutCountDay { get; set; }

    [Column("out_status_name")]
    public string? OutStatusName { get; set; }

    [Column("out_phone_number")]
    public string? OutPhoneNumber { get; set; }

    [Column("out_email_")]
    public string? OutEmail { get; set; }

    [Column("out_text_appeal")]
    public string? OutTextAppeal { get; set; }

    [Column("out_stage_name_current")]
    public string? OutStageNameCurrent { get; set; }

    [Column("out_employees_name_execution")]
    public string? OutEmployeesNameExecution { get; set; }

    [Column("out_employees_name_current")]
    public string? OutEmployeesNameCurrent { get; set; }

    [Column("out_category")]
    public string? OutCategory { get; set; }

    [Column("out_subject_treatment")]
    public string? OutSubjectTreatment { get; set; }

    [Column("out_type_difficulty")]
    public string? OutTypeDifficulty { get; set; }

    [Column("out_type")]
    public string? OutType { get; set; }

    [Column("out_mfc_name")]
    public string? OutMfcName { get; set; }

    [Column("out_date_regulation")]
    public DateTime? OutDateRegulation { get; set; }

    [Column("out_date_execution")]
    public DateTime? OutDateExecution { get; set; }

    [Column("out_case_number")]
    public string? OutCaseNumber { get; set; }
}