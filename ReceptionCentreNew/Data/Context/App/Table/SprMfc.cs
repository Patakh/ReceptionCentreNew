using System;
using System.Collections.Generic;

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
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// кто добавил запись
    /// </summary>
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// дата и время изменений
    /// </summary>
    public DateTime? DateModify { get; set; }

    /// <summary>
    /// кто изменил запись
    /// </summary>
    public string? EmployeesNameModify { get; set; }

    /// <summary>
    /// комментарий при изменении
    /// </summary>
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
    public bool IsRemove { get; set; }

    public virtual ICollection<DataAppeal> DataAppeal { get; set; } = new List<DataAppeal>();
}
