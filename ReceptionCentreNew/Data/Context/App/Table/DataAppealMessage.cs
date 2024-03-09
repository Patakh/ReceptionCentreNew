using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Сообщения заявителям
/// </summary>
public partial class DataAppealMessage
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    [Display(Name = "ID обращения")]
    public Guid Id { get; set; }

    /// <summary>
    /// Обращение
    /// </summary>
    public Guid? DataAppealId { get; set; }

    /// <summary>
    /// номер телефона
    /// </summary>
    [Display(Name = "Номер телефона")]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// сотрудник
    /// </summary>
    public Guid? SprEmployeesId { get; set; }

    /// <summary>
    /// фио сотрудника
    /// </summary>
    [Display(Name = "Сотрудник")]
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// дата и время добавления записи, отправки сообщения
    /// </summary>
    [Display(Name = "Дата")]
    public DateTime DateMessage { get; set; }

    /// <summary>
    /// текст сообщения
    /// </summary>
    [Display(Name = "Текст сообщения")]
    public string TextMessage { get; set; } = null!;

    /// <summary>
    /// тип сообщения (1-исходящий 2-входящий)
    /// </summary>
    [Display(Name = "Тип")]
    public short? MessageType { get; set; }

    public virtual DataAppeal? DataAppeal { get; set; }

    public virtual SprEmployees? SprEmployees { get; set; }
}
