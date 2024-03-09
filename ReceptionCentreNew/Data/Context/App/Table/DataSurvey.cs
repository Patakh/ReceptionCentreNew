using System.ComponentModel.DataAnnotations;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Вопросы клиентов
/// </summary>
public partial class DataSurvey
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Вопрос
    /// </summary>
    [Display(Name = "Справочник вопросов")]
    public Guid SprSurveyQuestionId { get; set; }

    /// <summary>
    /// Ответ
    /// </summary>
    [Display(Name = "Справочник вариантов ответов")]
    public Guid SprSurveyAnswerId { get; set; }

    /// <summary>
    /// Текст вопроса
    /// </summary>
    [Display(Name = "Вопрос")]
    public string? Question { get; set; }

    /// <summary>
    /// Текст ответа
    /// </summary>
    [Display(Name = "Ответ")]
    public string? Answer { get; set; }

    /// <summary>
    /// Номер телефона
    /// </summary>
    [Display(Name = "Номер телефона")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Дата опроса
    /// </summary>
    [Display(Name = "Дата опроса")]
    public DateTime? DateSurvey { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    [Display(Name = "Сотрудник")]
    public Guid? SprEmployeesId { get; set; }

    /// <summary>
    /// Кто добавил запись
    /// </summary>
    [Display(Name = "Сотрудник")]
    public string? EmployeesNameAdd { get; set; }

    /// <summary>
    /// ip адрес добавившего
    /// </summary>
    [Display(Name = "ip адрес")]
    public string? IpAddressAdd { get; set; }

    public virtual SprEmployees? SprEmployees { get; set; }

    public virtual SprSurveyAnswer SprSurveyAnswer { get; set; } = null!;

    public virtual SprSurveyQuestion SprSurveyQuestion { get; set; } = null!;
}
