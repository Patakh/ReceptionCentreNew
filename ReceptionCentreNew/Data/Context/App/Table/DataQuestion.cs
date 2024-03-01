using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

/// <summary>
/// Ответы клиентов
/// </summary>
public partial class DataQuestion
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Справочник консультаций
    /// </summary>
    public Guid SprQuestionId { get; set; }

    /// <summary>
    /// Дата ответа
    /// </summary>
    public DateTime? DateQuestion { get; set; }

    /// <summary>
    /// Номер ответа
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    public Guid? SprEmployeesId { get; set; }

    /// <summary>
    /// Кто добавил
    /// </summary>
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// ip адрес добавившего
    /// </summary>
    public string? IpAddressAdd { get; set; }

    public virtual SprEmployees? SprEmployees { get; set; }

    public virtual SprQuestion SprQuestion { get; set; } = null!;
}
