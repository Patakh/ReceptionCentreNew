using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App.Table;

/// <summary>
/// Порядок этапов
/// </summary>
public partial class SprRoutesStageNext
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Этап
    /// </summary>
    public int SprRoutesStageId { get; set; }

    /// <summary>
    /// Следующий этап
    /// </summary>
    public int SprRoutesStageIdNext { get; set; }

    public virtual SprRoutesStage SprRoutesStage { get; set; } = null!;

    public virtual SprRoutesStage SprRoutesStageIdNextNavigation { get; set; } = null!;
}
