using System;
using System.Collections.Generic;

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
    public Guid? DataAppealId { get; set; }

    /// <summary>
    /// Номер телефона
    /// </summary>
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Дата и время звонка
    /// </summary>
    public DateTime? DateCall { get; set; }

    /// <summary>
    /// Время разговора
    /// </summary>
    public string? TimeTalk { get; set; }

    /// <summary>
    /// Признак сохранения на ftp
    /// </summary>
    public bool SaveFtp { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    public Guid? SprEmployeesId { get; set; }

    /// <summary>
    /// ФИО сотрудника
    /// </summary>
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// тип звонка (1-исходящий 2-входящий)
    /// </summary>
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
