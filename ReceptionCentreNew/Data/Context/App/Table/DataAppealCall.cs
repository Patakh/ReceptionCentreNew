using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Звонки
/// </summary>
public partial class DataAppealCall
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Обращение
    /// </summary>
    [Display(Name = "ID обращения")]
    public Guid? DataAppealId { get; set; }

    /// <summary>
    /// Номер телефона
    /// </summary>
    [Display(Name = "Телефон")]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Дата и время звонка
    /// </summary>
    [Display(Name = "Дата звонка")]
    public DateTime? DateCall { get; set; }

    /// <summary>
    /// Время разговора
    /// </summary>
    [Display(Name = "Продолжительность")]
    public string? TimeTalk { get; set; }

    /// <summary>
    /// Признак сохранения на ftp
    /// </summary>
    [Display(Name = "ftp")]
    public bool SaveFtp { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    [Display(Name = "Сотрудник")]
    public Guid? SprEmployeesId { get; set; }

    /// <summary>
    /// ФИО сотрудника
    /// </summary>
    [Display(Name = "Сотрудник")]
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// тип звонка (1-исходящий 2-входящий)
    /// </summary>
    [Display(Name = "Тип")]
    public short CallType { get; set; }

    /// <summary>
    /// id звонка jitsi
    /// </summary>
    public string? PeerId { get; set; }

    /// <summary>
    /// Признак удаления
    /// </summary>
    public bool IsChecked { get; set; }

    public virtual DataAppeal? DataAppeal { get; set; }

    public virtual SprEmployees? SprEmployees { get; set; }
}
