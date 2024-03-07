using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Data.Context.App;
public partial class DataAppealRouteStageSelect
{
    [Display(Name = "Id этапа")]
    [Column("out_id")]
    public Guid OutId { get; set; }

    [Display(Name = "Этап")]
    [Column("out_stage_name")]
    public string OutStageName { get; set; }

    [Display(Name = "Дата")]
    [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
    [Column("out_date_start")]
    public DateTime? OutDateStart { get; set; }

    [Display(Name = "Время")]
    [DisplayFormat(DataFormatString = "{0:hh':'mm}", ApplyFormatInEditMode = true)]
    [Column("out_time_start")]
    public TimeSpan? OutTimeStart { get; set; }

    [Display(Name = "Дата")]
    [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
    [Column("out_date_stop")]
    public DateTime? OutDateStop { get; set; }

    [Display(Name = "Время")]
    [DisplayFormat(DataFormatString = "{0:hh':'mm}", ApplyFormatInEditMode = true)]
    [Column("out_time_stop")]
    public TimeSpan? OutTimeStop { get; set; }

    [Display(Name = "Сотрудник")]
    [Column("out_employees_name")]
    public string OutEmployeesName { get; set; }

    [Display(Name = "Кто добавил запись")]
    [Column("out_employees_name_add")]
    public string OutEmployeesNameAdd { get; set; }

    [Display(Name = "Комментарии")]
    [Column("out_commentt")]
    public string OutCommentt { get; set; }

    [Display(Name = "Дата регламентная")]
    [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
    [Column("out_date_regulation")]
    public DateTime? OutDateRegulation { get; set; }

    [Display(Name = "Дни (регл.)")]
    [Column("out_count_day_regulation")]
    public int? OutCountDayRegulation { get; set; }

    [Display(Name = "Дни (факт.)")]
    [Column("out_count_day_fact")]
    public string OutCountDayFact { get; set; }
}
