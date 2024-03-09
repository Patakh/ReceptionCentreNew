using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

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
    [Display(Name = "Справочник консультаций")]
    public Guid SprQuestionId { get; set; }

    /// <summary>
    /// Дата ответа
    /// </summary>
    [Display(Name = "Дата ответа")]
    public DateTime? DateQuestion { get; set; }

    /// <summary>
    /// Номер ответа
    /// </summary>
    [Display(Name = "Телефон")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    [Display(Name = "Сотрудник")]
    public Guid? SprEmployeesId { get; set; }

    /// <summary>
    /// Кто добавил
    /// </summary>
    [Display(Name = "Сотрудник")]
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// ip адрес добавившего
    /// </summary>
    [Display(Name = "ip адрес")]
    public string? IpAddressAdd { get; set; }

    public virtual SprEmployees? SprEmployees { get; set; }

    public virtual SprQuestion SprQuestion { get; set; } = null!;
}
