using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Обращение
/// </summary>
public partial class DataAppeal
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    [Display(Name = "Сотрудник")]
    public Guid SprEmployeesId { get; set; }

    /// <summary>
    /// Тип
    /// </summary>
    [Display(Name = "Тип")]
    public Guid SprTypeId { get; set; }

    /// <summary>
    /// Предмет обращения
    [Display(Name = "Предмет")]
    /// </summary>
    public Guid SprSubjectTreatmentId { get; set; }

    /// <summary>
    /// Сложность
    /// </summary>
    [Display(Name = "Тип сложности")]
    public Guid SprTypeDifficultyId { get; set; }

    /// <summary>
    /// Категория
    /// </summary>
    [Display(Name = "Категория")]
    public Guid SprCategoryId { get; set; }

    /// <summary>
    /// Статус
    /// </summary>
    [Display(Name = "Статус")]
    public int SprStatusId { get; set; }

    /// <summary>
    /// Время обработки, дни
    /// </summary>
    [Display(Name = "Дней")]
    public int? CountDay { get; set; }

    /// <summary>
    /// ФИО заявителя
    /// </summary>
    [Display(Name = "ФИО заявителя")]
    public string? ApplicantName { get; set; }

    /// <summary>
    /// Номер телефона
    /// </summary>
    [Display(Name = "Номер телефона")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Электронная почта
    /// </summary>
    [Display(Name = "Электронная почта")]
    public string? Email { get; set; }

    /// <summary>
    /// Регламентный срок
    /// </summary>
    [Display(Name = "Регламентный срок")]
    public DateTime? DateRegulation { get; set; }

    /// <summary>
    /// Текст обращения
    /// </summary>
    [Display(Name = "Текст обращения")]
    public string? TextAppeal { get; set; }

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
    /// текущий этап
    /// </summary>
    [Display(Name = "Текущий этап")]
    public int? SprRoutesStageIdCurrent { get; set; }

    /// <summary>
    /// текущий сотрудник
    /// </summary>
    [Display(Name = "Текущий cотрудник")]
    public Guid? SprEmployeesIdCurrent { get; set; }

    /// <summary>
    /// дата и время исполнения
    /// </summary>
    [Display(Name = "Дата исполнения")]
    public DateTime? DateExecution { get; set; }

    /// <summary>
    /// исполнитель
    /// </summary>
    [Display(Name = "Исполнитель")]
    public Guid? SprEmployeesIdExecution { get; set; }

    /// <summary>
    /// номер обращения
    /// </summary>
    [Display(Name = "Номер обращения")]
    public string NumberAppeal { get; set; } = null!;

    /// <summary>
    /// ip адрес добавившего
    /// </summary>
    public string? IpAddressAdd { get; set; }

    /// <summary>
    /// ip адрес изменившего
    /// </summary>
    public string? IpAddressModify { get; set; }

    /// <summary>
    /// Наименование мфц
    /// </summary>
    [Display(Name = "МФЦ")]
    public Guid? SprMfcId { get; set; }

    /// <summary>
    /// номер обращения в МФЦ
    /// </summary>
    [Display(Name = "Номер дела в МФЦ")]
    public string? CaseNumber { get; set; }

    public virtual ICollection<DataAppealCall> DataAppealCall { get; set; } = new List<DataAppealCall>();

    public virtual ICollection<DataAppealCommentt> DataAppealCommentt { get; set; } = new List<DataAppealCommentt>();

    public virtual ICollection<DataAppealEmail> DataAppealEmail { get; set; } = new List<DataAppealEmail>();

    public virtual ICollection<DataAppealFile> DataAppealFile { get; set; } = new List<DataAppealFile>();

    public virtual ICollection<DataAppealMessage> DataAppealMessage { get; set; } = new List<DataAppealMessage>();

    public virtual ICollection<DataAppealRoutesStage> DataAppealRoutesStage { get; set; } = new List<DataAppealRoutesStage>();

    public virtual ICollection<DataEmployeesNotification> DataEmployeesNotification { get; set; } = new List<DataEmployeesNotification>();

    public virtual SprCategory SprCategory { get; set; } = null!;

    public virtual SprEmployees SprEmployees { get; set; } = null!;

    public virtual SprEmployees? SprEmployeesIdCurrentNavigation { get; set; }

    public virtual SprEmployees? SprEmployeesIdExecutionNavigation { get; set; }

    public virtual SprMfc? SprMfc { get; set; }

    public virtual SprRoutesStage? SprRoutesStageIdCurrentNavigation { get; set; }

    public virtual SprStatus SprStatus { get; set; } = null!;

    public virtual SprSubjectTreatment SprSubjectTreatment { get; set; } = null!;

    public virtual SprType SprType { get; set; } = null!;

    public virtual SprTypeDifficulty SprTypeDifficulty { get; set; } = null!;
}
