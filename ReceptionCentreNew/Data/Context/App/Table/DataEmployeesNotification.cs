using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

/// <summary>
/// Уведомления сотрудником
/// </summary>
public partial class DataEmployeesNotification
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
    /// Тип уведомления
    /// </summary>
    public int SprNotificationId { get; set; }

    /// <summary>
    /// Обращение
    /// </summary>
    public Guid DataAppealId { get; set; }

    /// <summary>
    /// Дата и время поступления
    /// </summary>
    public DateTime DateReceipt { get; set; }

    /// <summary>
    /// Активно/неактивно
    /// </summary>
    public bool? IsActive { get; set; }

    public virtual DataAppeal DataAppeal { get; set; } = null!;

    public virtual SprEmployees SprEmployees { get; set; } = null!;

    public virtual SprNotification SprNotification { get; set; } = null!;
}
