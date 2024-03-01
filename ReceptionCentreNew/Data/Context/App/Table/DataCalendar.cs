using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

/// <summary>
/// Календарь
/// </summary>
public partial class DataCalendar
{
    /// <summary>
    /// Первичный ключ (дата)
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Тип дня, 1 - рабочие день, 0 - (Суббота Воскресенье), 2 (Праздничный день)
    /// </summary>
    public int DateType { get; set; }

    /// <summary>
    /// Признак удаления
    /// </summary>
    public bool? IsRemove { get; set; }

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
    /// ip адрес добавившего
    /// </summary>
    public string? IpAddressAdd { get; set; }

    /// <summary>
    /// ip адрес изменившего
    /// </summary>
    public string? IpAddressModify { get; set; }
}
