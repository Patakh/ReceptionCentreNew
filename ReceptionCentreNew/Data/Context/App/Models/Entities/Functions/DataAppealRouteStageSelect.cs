using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Domain.Concrete;
public partial class DataAppealRouteStageSelect
{
    [Display(Name = "Id этапа")]
    public Guid OutId { get; set; }

    [Display(Name = "Этап")]
    public string OutStageName { get; set; }

    [Display(Name = "Дата")]
    [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? OutDateStart { get; set; }

    [Display(Name = "Время")]
    [DisplayFormat(DataFormatString = "{0:hh':'mm}", ApplyFormatInEditMode = true)]
    public TimeSpan? OutTimeStart { get; set; }

    [Display(Name = "Дата")]
    [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? OutDateStop { get; set; }

    [Display(Name = "Время")]
    [DisplayFormat(DataFormatString = "{0:hh':'mm}", ApplyFormatInEditMode = true)]
    public TimeSpan? OutTimeStop { get; set; }

    [Display(Name = "Сотрудник")]
    public string OutEmployeesName { get; set; }

    [Display(Name = "Кто добавил запись")]
    public string OutEmployeesNameAdd { get; set; }

    [Display(Name = "Комментарии")]
    public string OutCommentt { get; set; }

    [Display(Name = "Дата регламентная")]
    [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? OutDateRegulation { get; set; }

    [Display(Name = "Дни (регл.)")]
    public int? OutCountDayRegulation { get; set; }

    [Display(Name = "Дни (факт.)")]
    public string OutCountDayFact { get; set; }
}
