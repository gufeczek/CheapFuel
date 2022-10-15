using Domain.Common.Interfaces;

namespace Domain.Entities;

public class OpeningClosingTime : ITracked
{
    public long FuelStationId { get; set; }
    public FuelStation? FuelStation { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public int? OpeningTime { get; set; }
    public int? ClosingTime { get; set; }
    
    public long UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}