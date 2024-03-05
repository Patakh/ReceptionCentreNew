using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Сообщения заявителям
/// </summary>
public partial class DataAppealMessage
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
    /// номер телефона
    /// </summary>
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// сотрудник
    /// </summary>
    public Guid? SprEmployeesId { get; set; }

    /// <summary>
    /// фио сотрудника
    /// </summary>
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// дата и время добавления записи, отправки сообщения
    /// </summary>
    public DateTime DateMessage { get; set; }

    /// <summary>
    /// текст сообщения
    /// </summary>
    public string TextMessage { get; set; } = null!;

    /// <summary>
    /// тип сообщения (1-исходящий 2-входящий)
    /// </summary>
    public short? MessageType { get; set; }

    public virtual DataAppeal? DataAppeal { get; set; }

    public virtual SprEmployees? SprEmployees { get; set; }
}
