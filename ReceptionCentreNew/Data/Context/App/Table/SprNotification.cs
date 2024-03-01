using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

/// <summary>
/// Типы уводемлений
/// </summary>
public partial class SprNotification
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Наименование типа уведомлений
    /// </summary>
    public string Notification { get; set; } = null!;

    public virtual ICollection<DataEmployeesNotification> DataEmployeesNotification { get; set; } = new List<DataEmployeesNotification>();
}
