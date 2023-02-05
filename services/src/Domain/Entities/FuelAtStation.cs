using Domain.Common.Interfaces;

namespace Domain.Entities;

public class FuelAtStation : ICreatable
{
    public long FuelTypeId { get; set; }
    public FuelType? FuelType { get; set; }
    
    public long FuelStationId { get; set; }
    public FuelStation? FuelStation { get; set; }
    
    public long? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}