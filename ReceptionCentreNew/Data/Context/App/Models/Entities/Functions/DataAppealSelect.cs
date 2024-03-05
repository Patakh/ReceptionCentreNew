namespace ReceptionCentreNew.Domain.Concrete;
using System;
using System.ComponentModel.DataAnnotations;
public partial class DataAppealSelect
{
    public Guid OutDataAppealId { get; set; }

    public Guid? OutSprEmployeesId { get; set; }

    public string OutNumberAppeal { get; set; }

    public string OutEmployeesName { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy HH':'mm}", ApplyFormatInEditMode = true)]
    public DateTime? OutDateAdd { get; set; }

    public string OutApplicantName { get; set; }

    public int? OutCountDay { get; set; }

    public string OutStatusName { get; set; }

    public string OutPhoneNumber { get; set; }

    public string OutEmail { get; set; }

    public string OutTextAppeal { get; set; }

    public string OutStageNameCurrent { get; set; }

    public string OutEmployeesNameExecution { get; set; }

    public string OutEmployeesNameCurrent { get; set; }

    public string OutCategory { get; set; }

    public string OutSubjectTreatment { get; set; }

    public string OutTypeDifficulty { get; set; }

    public string OutType { get; set; }

    public string OutMfcName { get; set; }

    public DateTime? OutDateRegulation { get; set; }

    public DateTime? OutDateExecution { get; set; }

    public string OutCaseNumber { get; set; }
}