using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Должность
/// </summary>
public partial class SprEmployeesJobPos
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Наименование должности
    /// </summary>
    [Display(Name = "Должность")]
    public string JobPosName { get; set; } = null!;

    /// <summary>
    /// Признак удаления
    /// </summary>
    [Display(Name = "Признак удаления")]
    public bool IsRemove { get; set; }

    /// <summary>
    /// Кто добавил запись
    /// </summary>
    [Display(Name = "Добавил")]
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// Дата и время добавления записи
    /// </summary>
    [Display(Name = "Дата добавления")]
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// Кто изменил запись
    /// </summary>
    [Display(Name = "Изменил")]
    public string? EmployeesNameModify { get; set; }

    /// <summary>
    /// Дата и время последних изменений
    /// </summary>
    [Display(Name = "Дата изменения")]
    public DateTime? DateModify { get; set; }

    /// <summary>
    /// Комментарий при изменении
    /// </summary>
    [Display(Name = "Причина изменения")]
    public string? CommenttModify { get; set; }

    /// <summary>
    /// ip адрес добавившего
    /// </summary>
    public string? IpAddressAdd { get; set; }

    /// <summary>
    /// ip адрес изменившего
    /// </summary>
    public string? IpAddressModify { get; set; }

    public virtual ICollection<SprEmployees> SprEmployees { get; set; } = new List<SprEmployees>();
}
