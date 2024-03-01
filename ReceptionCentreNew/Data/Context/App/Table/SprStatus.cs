using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

/// <summary>
/// Статусы
/// </summary>
public partial class SprStatus
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Наименование статуса
    /// </summary>
    public string StatusName { get; set; } = null!;

    public virtual ICollection<DataAppeal> DataAppeal { get; set; } = new List<DataAppeal>();
}
