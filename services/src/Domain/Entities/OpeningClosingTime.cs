using Domain.Common;

namespace Domain.Entities;

public class OpeningClosingTime : AuditableKeylessEntity
{
    public long FuelStationId { get; set; }
    public FuelStation? FuelStation { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public int? OpeningTime { get; set; }
    public int? ClosingTime { get; set; }
}