using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Таблица хранения заказных звонков
/// </summary>
public partial class DataCallback
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Дата на которую заказан звонок
    /// </summary>
    [Display(Name = "Дата на которую заказан звонок")]
    public DateTime DateOrder { get; set; }

    /// <summary>
    /// Номер телефона заявителя
    /// </summary>
    [Display(Name = "Номер телефона заявителя")]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Владелец номера телефона
    /// </summary>
    [Display(Name = "Владелец номера телефона")]
    public string? CustomerFio { get; set; }

    /// <summary>
    /// Статус звонка (1 - Новый звонок, 2 - Обработан, 3 - Не отвеченный)
    /// </summary>
    [Display(Name = "Статус звонка (1 - Новый звонок, 2 - Обработан, 3 - Не отвеченный)")]
    public int Status { get; set; }

    /// <summary>
    /// Количество попыток
    /// </summary>
    [Display(Name = "Количество попыток")]
    public int CountTry { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    public Guid? SprEmployeesId { get; set; }

    /// <summary>
    /// ID звонка
    /// </summary>
    public int CallbackId { get; set; }

    /// <summary>
    /// Полное занесение звонка  в базу
    /// </summary>
    public bool IsSync { get; set; }

    /// <summary>
    /// Дата и время занесения записи
    /// </summary>
    public DateTime DateAdd { get; set; }

    /// <summary>
    /// Дата закрытия
    /// </summary>
    public DateOnly? DateClose { get; set; }

    /// <summary>
    /// В ручную?
    /// </summary>
    public bool? IsHand { get; set; }

    /// <summary>
    /// Сотрудник закрывший
    /// </summary>
    public Guid? SprEmployeesIdClose { get; set; }

    public virtual ICollection<DataCallbackCalls> DataCallbackCalls { get; set; } = new List<DataCallbackCalls>();

    public virtual SprEmployees? SprEmployees { get; set; }

    public virtual SprEmployees? SprEmployeesIdCloseNavigation { get; set; }
}
