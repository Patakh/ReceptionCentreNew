using System;
using System.Collections.Generic;

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
    public Guid SprEmployeesId { get; set; }

    /// <summary>
    /// Тип
    /// </summary>
    public Guid SprTypeId { get; set; }

    /// <summary>
    /// Предмет обращения
    /// </summary>
    public Guid SprSubjectTreatmentId { get; set; }

    /// <summary>
    /// Сложность
    /// </summary>
    public Guid SprTypeDifficultyId { get; set; }

    /// <summary>
    /// Категория
    /// </summary>
    public Guid SprCategoryId { get; set; }

    /// <summary>
    /// Статус
    /// </summary>
    public int SprStatusId { get; set; }

    /// <summary>
    /// Время обработки, дни
    /// </summary>
    public int? CountDay { get; set; }

    /// <summary>
    /// ФИО заявителя
    /// </summary>
    public string? ApplicantName { get; set; }

    /// <summary>
    /// Номер телефона
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Электронная почта
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Регламентный срок
    /// </summary>
    public DateTime? DateRegulation { get; set; }

    /// <summary>
    /// Текст обращения
    /// </summary>
    public string? TextAppeal { get; set; }

    /// <summary>
    /// Признак удаления
    /// </summary>
    public bool IsRemove { get; set; }

    /// <summary>
    /// Кто добавил запись
    /// </summary>
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// Дата и время добавления записи
    /// </summary>
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// Кто изменил запись
    /// </summary>
    public string? EmployeesNameModify { get; set; }

    /// <summary>
    /// Дата и время последних изменений
    /// </summary>
    public DateTime? DateModify { get; set; }

    /// <summary>
    /// Комментарий при изменении
    /// </summary>
    public string? CommenttModify { get; set; }

    /// <summary>
    /// текущий этап
    /// </summary>
    public int? SprRoutesStageIdCurrent { get; set; }

    /// <summary>
    /// текущий сотрудник
    /// </summary>
    public Guid? SprEmployeesIdCurrent { get; set; }

    /// <summary>
    /// дата и время исполнения
    /// </summary>
    public DateTime? DateExecution { get; set; }

    /// <summary>
    /// исполнитель
    /// </summary>
    public Guid? SprEmployeesIdExecution { get; set; }

    /// <summary>
    /// номер обращения
    /// </summary>
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
    public Guid? SprMfcId { get; set; }

    /// <summary>
    /// номер обращения в МФЦ
    /// </summary>
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
