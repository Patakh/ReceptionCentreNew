using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

/// <summary>
/// Настройки
/// </summary>
public partial class SprSetting
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Переменная
    /// </summary>
    public string ParamName { get; set; } = null!;

    /// <summary>
    /// Значение переменной
    /// </summary>
    public string ParamValue { get; set; } = null!;

    /// <summary>
    /// Комментарий
    /// </summary>
    public string? Commentt { get; set; }
}
