using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class DataAppealRouteStageNext
{
    [Column("out_spr_routes_stage_id")]
    public int OutSprRoutesStageId { get; set; }

    [Column("out_stage_name")]
    public string OutStageName { get; set; }
}