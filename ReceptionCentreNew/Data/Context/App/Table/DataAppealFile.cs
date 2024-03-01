using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

/// <summary>
/// Файлы к обращению
/// </summary>
public partial class DataAppealFile
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Обращение
    /// </summary>
    public Guid? DataAppealId { get; set; }

    /// <summary>
    /// имя файла
    /// </summary>
    public string FileName { get; set; } = null!;

    /// <summary>
    /// расширение файла
    /// </summary>
    public string FileExt { get; set; } = null!;

    /// <summary>
    /// размер файла
    /// </summary>
    public int FileSize { get; set; }

    /// <summary>
    /// Признак удаления
    /// </summary>
    public bool IsRemove { get; set; }

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

    /// <summary>
    /// Сотрудник
    /// </summary>
    public Guid? SprEmployeesId { get; set; }

    public virtual DataAppeal? DataAppeal { get; set; }

    public virtual SprEmployees? SprEmployees { get; set; }
}
