using Domain.Common;

namespace Domain.Entities;

public class FuelAtStation
{
    public long FuelTypeId { get; set; }
    public FuelType? FuelType { get; set; }
    
    public long FuelStationId { get; set; }
    public FuelStation? FuelStation { get; set; }
}