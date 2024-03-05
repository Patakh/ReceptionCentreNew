namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class StatisticsDataAppealCall
{
    public string? OutMonth { get; set; }
    public int OutCountCallIncoming { get; set; }
    public int OutCountCallOutgoing { get; set; }
    public int OutCountCallMissed { get; set; }
    public int OutYear { get; set; }
}