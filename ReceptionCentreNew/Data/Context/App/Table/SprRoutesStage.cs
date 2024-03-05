using System;
using System.Collections.Generic;

namespace ReceptionCentreNew.Data.Context.App;

/// <summary>
/// Этапы
/// </summary>
public partial class SprRoutesStage
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Наименование этапа
    /// </summary>
    public string StageName { get; set; } = null!;

    public virtual ICollection<DataAppeal> DataAppeal { get; set; } = new List<DataAppeal>();

    public virtual ICollection<DataAppealRoutesStage> DataAppealRoutesStage { get; set; } = new List<DataAppealRoutesStage>();

    public virtual ICollection<SprRoutesStageNext> SprRoutesStageNextSprRoutesStage { get; set; } = new List<SprRoutesStageNext>();

    public virtual ICollection<SprRoutesStageNext> SprRoutesStageNextSprRoutesStageIdNextNavigation { get; set; } = new List<SprRoutesStageNext>();
}
