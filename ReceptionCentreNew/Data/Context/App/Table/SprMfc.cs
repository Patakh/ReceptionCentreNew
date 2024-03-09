using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Справочник мфц
/// </summary>
public partial class SprMfc
{
    /// <summary>
    /// первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// нименование мфц
    /// </summary>
    public string MfcName { get; set; } = null!;

    /// <summary>
    /// краткое наименование мфц
    /// </summary>
    public string MfcNameSmall { get; set; } = null!;

    /// <summary>
    /// дата и время добавления записи
    /// </summary>
    [Display(Name = "Дата добавления")]
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// кто добавил запись
    /// </summary>
    [Display(Name = "Добавил")]
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// дата и время изменений
    /// </summary>
    [Display(Name = "Дата изменения")]
    public DateTime? DateModify { get; set; }

    /// <summary>
    /// кто изменил запись
    /// </summary>
    [Display(Name = "Изменил")]
    public string? EmployeesNameModify { get; set; }

    /// <summary>
    /// комментарий при изменении
    /// </summary>
    [Display(Name = "Причина изменения")]
    public string? CommenttModify { get; set; }

    /// <summary>
    /// ip адрес добавившего запись
    /// </summary>
    public string? IpAddressAdd { get; set; }

    /// <summary>
    /// ip адрес изменившего запись
    /// </summary>
    public string? IpAddressModify { get; set; }

    /// <summary>
    /// признак удаления
    /// </summary>
    [Display(Name = "Признак удаления")]
    public bool IsRemove { get; set; }

    public virtual ICollection<DataAppeal> DataAppeal { get; set; } = new List<DataAppeal>();
}
