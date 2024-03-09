using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    [Display(Name = "Таблица")]
    public string TableName { get; set; } = null!;

    /// <summary>
    /// наименование поля
    /// </summary>
    [Display(Name = "Поле")]
    public string FieldName { get; set; } = null!;

    /// <summary>
    /// старое значение
    /// </summary>
    [Display(Name = "Старое значение")]
    public string? OldValue { get; set; }

    /// <summary>
    /// новое значение
    /// </summary>
    [Display(Name = "Новое значение")]
    public string? NewValue { get; set; }

    /// <summary>
    /// сторудник
    /// </summary>
    [Display(Name = "Сотрудник")]
    public string EmployeesName { get; set; } = null!;

    /// <summary>
    /// дата и время изменений
    /// </summary>
    [Display(Name = "Дата")]
    public DateTime DateChange { get; set; }

    /// <summary>
    /// комментарий
    /// </summary>
    [Display(Name = "Комментарии")]
    public string? Commentt { get; set; }

    /// <summary>
    /// наименование таблицы в базе
    /// </summary>
    [Display(Name = "Наименование таблицы в базе")]
    public string? TableNameBase { get; set; }

    /// <summary>
    /// наименование поля в базе
    /// </summary>
    [Display(Name = "Наименование поля в базе")]
    public string? FieldNameBase { get; set; }

    [Display(Name = "ИД записи")]
    public Guid RowId { get; set; }

    /// <summary>
    /// ip адрес
    /// </summary>
    public string? IpAddress { get; set; }
}
