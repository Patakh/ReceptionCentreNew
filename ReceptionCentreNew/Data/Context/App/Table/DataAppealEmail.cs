using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

/// <summary>
/// Электронная почта
/// </summary>
public partial class DataAppealEmail
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// id обращения
    /// </summary>
    public Guid? DataAppealId { get; set; }

    /// <summary>
    /// электронная почта
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// id сотрудника
    /// </summary>
    public Guid? SprEmployeesId { get; set; }

    /// <summary>
    /// фио сотрудника
    /// </summary>
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// дата и время письма
    /// </summary>
    public DateTime DateEmail { get; set; }

    /// <summary>
    /// Текст письма
    /// </summary>
    public string? TextEmail { get; set; }

    /// <summary>
    /// 1 исходящий 2 входящий
    /// </summary>
    public short EmailType { get; set; }

    /// <summary>
    /// идентификатор сообщения на почте
    /// </summary>
    public string? Uids { get; set; }

    /// <summary>
    /// false - не прочитано, true - прочитано
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Заголовок
    /// </summary>
    public string? Caption { get; set; }

    public virtual DataAppeal? DataAppeal { get; set; }

    public virtual SprEmployees? SprEmployees { get; set; }
}
