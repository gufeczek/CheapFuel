using Domain.Common;

namespace Domain.Entities;

public class ServiceAtStation
{
    public long ServiceId { get; set; }
    public Service? Service { get; set; }

    public long FuelStationId { get; set; }
    public FuelStation? FuelStation { get; set; }
}