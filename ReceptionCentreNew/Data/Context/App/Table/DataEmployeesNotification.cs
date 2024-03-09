using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Уведомления сотрудником
/// </summary>
public partial class DataEmployeesNotification
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    [Display(Name ="Id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    [Display(Name = "Сотрудник")]
    public Guid SprEmployeesId { get; set; }

    /// <summary>
    /// Тип уведомления
    /// </summary>
    [Display(Name = "Тип уведомления")]
    public int SprNotificationId { get; set; }

    /// <summary>
    /// Обращение
    /// </summary>
    [Display(Name = "Обращение")]
    public Guid DataAppealId { get; set; }

    /// <summary>
    /// Дата и время поступления
    /// </summary>
    [Display(Name = "Дата")]
    public DateTime DateReceipt { get; set; }

    /// <summary>
    /// Активно/неактивно
    /// </summary>
    [Display(Name = "Активно/неактивно")]
    public bool? IsActive { get; set; }

    public virtual DataAppeal DataAppeal { get; set; } = null!;

    public virtual SprEmployees SprEmployees { get; set; } = null!;

    public virtual SprNotification SprNotification { get; set; } = null!;
}
