using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class StatisticsDataAppealSubject
{
    [Column("out_count_appeal")]
    public int OutCountAppeal { get; set; }

    [Column("out_subject_name")]
    public string? OutSubjectName { get; set; }
}