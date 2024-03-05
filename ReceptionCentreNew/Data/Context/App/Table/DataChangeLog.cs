using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// История изменений
/// </summary>
public partial class DataChangeLog
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// наименование таблицы
    /// </summary>
    public string TableName { get; set; } = null!;

    /// <summary>
    /// наименование поля
    /// </summary>
    public string FieldName { get; set; } = null!;

    /// <summary>
    /// старое значение
    /// </summary>
    public string? OldValue { get; set; }

    /// <summary>
    /// новое значение
    /// </summary>
    public string? NewValue { get; set; }

    /// <summary>
    /// сторудник
    /// </summary>
    public string EmployeesName { get; set; } = null!;

    /// <summary>
    /// дата и время изменений
    /// </summary>
    public DateTime DateChange { get; set; }

    /// <summary>
    /// комментарий
    /// </summary>
    public string? Commentt { get; set; }

    /// <summary>
    /// наименование таблицы в базе
    /// </summary>
    public string? TableNameBase { get; set; }

    /// <summary>
    /// наименование поля в базе
    /// </summary>
    public string? FieldNameBase { get; set; }

    public Guid RowId { get; set; }

    /// <summary>
    /// ip адрес
    /// </summary>
    public string? IpAddress { get; set; }
}
