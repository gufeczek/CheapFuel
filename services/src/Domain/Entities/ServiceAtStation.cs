using Domain.Common.Interfaces;

namespace Domain.Entities;

public class ServiceAtStation : ICreatable
{
    public long ServiceId { get; set; }
    public FuelStationService? Service { get; set; }

    public long FuelStationId { get; set; }
    public FuelStation? FuelStation { get; set; }
    
    public long? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}