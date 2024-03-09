using ReceptionCentreNew.Models;
using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Сотрудники
/// </summary>
public partial class SprEmployees  
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Отдел
    /// </summary>
    [Display(Name = "Отдел")]
    public Guid SprEmployeesDepartmentId { get; set; }

    /// <summary>
    /// Должность
    /// </summary>
    [Display(Name = "Должность")]
    public Guid SprEmployeesJobPosId { get; set; }

    /// <summary>
    /// ФИО сотрудника
    /// </summary>
    [Display(Name = "ФИО")]
    public string EmployeesName { get; set; } = null!;

    /// <summary>
    /// Логин сотрудника
    /// </summary>
    [Display(Name = "Логин")]
    public string EmployeesLogin { get; set; } = null!;

    /// <summary>
    /// Пароль
    /// </summary>
    [Display(Name = "Пароль")]
    public string? EmployeesPass { get; set; }

    /// <summary>
    /// Активность сотрудника
    /// </summary>
    [Display(Name = "Признак активности")]
    public bool? EmployeesActive { get; set; }

    /// <summary>
    /// Признак удаления
    /// </summary>
    [Display(Name = "Признак удаления")]
    public bool IsRemove { get; set; }

    /// <summary>
    /// Кто добавил запись
    /// </summary>
    [Display(Name = "Добавил")]
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// Дата и время добавления записи
    /// </summary>
    [Display(Name = "Дата добавления")]
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// Кто изменил запись
    /// </summary>
    [Display(Name = "Изменил")]
    public string? EmployeesNameModify { get; set; }

    /// <summary>
    /// Дата и время последних изменений
    /// </summary>
    [Display(Name = "Дата изменения")]
    public DateTime? DateModify { get; set; }

    /// <summary>
    /// Комментарий при изменении
    /// </summary>
    [Display(Name = "Причина изменения")]
    public string? CommenttModify { get; set; }

    /// <summary>
    /// может принимать дела
    /// </summary>
    [Display(Name = "Может принимать дела")]
    public bool CanTakeAppeal { get; set; }

    /// <summary>
    /// ip адрес добавившего
    /// </summary>
    public string? IpAddressAdd { get; set; }

    /// <summary>
    /// ip адрес изменившего
    /// </summary>
    public string? IpAddressModify { get; set; }

    public virtual ICollection<DataAppealCall> DataAppealCall { get; set; } = new List<DataAppealCall>();

    public virtual ICollection<DataAppealCommentt> DataAppealCommentt { get; set; } = new List<DataAppealCommentt>();

    public virtual ICollection<DataAppealCommenttRecipients> DataAppealCommenttRecipients { get; set; } = new List<DataAppealCommenttRecipients>();

    public virtual ICollection<DataAppealEmail> DataAppealEmail { get; set; } = new List<DataAppealEmail>();

    public virtual ICollection<DataAppealFile> DataAppealFile { get; set; } = new List<DataAppealFile>();

    public virtual ICollection<DataAppealMessage> DataAppealMessage { get; set; } = new List<DataAppealMessage>();

    public virtual ICollection<DataAppealRoutesStage> DataAppealRoutesStage { get; set; } = new List<DataAppealRoutesStage>();

    public virtual ICollection<DataAppeal> DataAppealSprEmployees { get; set; } = new List<DataAppeal>();

    public virtual ICollection<DataAppeal> DataAppealSprEmployeesIdCurrentNavigation { get; set; } = new List<DataAppeal>();

    public virtual ICollection<DataAppeal> DataAppealSprEmployeesIdExecutionNavigation { get; set; } = new List<DataAppeal>();

    public virtual ICollection<DataCallbackCalls> DataCallbackCalls { get; set; } = new List<DataCallbackCalls>();

    public virtual ICollection<DataCallback> DataCallbackSprEmployees { get; set; } = new List<DataCallback>();

    public virtual ICollection<DataCallback> DataCallbackSprEmployeesIdCloseNavigation { get; set; } = new List<DataCallback>();

    public virtual ICollection<DataEmployeesNotification> DataEmployeesNotification { get; set; } = new List<DataEmployeesNotification>();

    public virtual ICollection<DataQuestion> DataQuestion { get; set; } = new List<DataQuestion>();

    public virtual ICollection<DataSurvey> DataSurvey { get; set; } = new List<DataSurvey>();

    public virtual SprEmployeesDepartment SprEmployeesDepartment { get; set; } = null!;

    public virtual SprEmployeesJobPos SprEmployeesJobPos { get; set; } = null!;

    public virtual ICollection<SprEmployeesMessageTemplate> SprEmployeesMessageTemplate { get; set; } = new List<SprEmployeesMessageTemplate>();

    public virtual ICollection<SprEmployeesRoleJoin> SprEmployeesRoleJoin { get; set; } = new List<SprEmployeesRoleJoin>();

    public virtual ICollection<SprEmployeesTextAppealTemplate> SprEmployeesTextAppealTemplate { get; set; } = new List<SprEmployeesTextAppealTemplate>();
}
