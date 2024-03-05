using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Получатели примечаний
/// </summary>
public partial class DataAppealCommenttRecipients
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Примечание
    /// </summary>
    public Guid DataAppealCommenttId { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    public Guid SprEmployeesId { get; set; }

    /// <summary>
    /// Отметка о прочтении
    /// </summary>
    public bool? ReadMark { get; set; }

    /// <summary>
    /// Дата и время прочтения
    /// </summary>
    public DateTime? ReadDate { get; set; }

    public virtual DataAppealCommentt DataAppealCommentt { get; set; } = null!;

    public virtual SprEmployees SprEmployees { get; set; } = null!;
}
