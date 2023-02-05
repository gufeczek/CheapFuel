using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class FuelPrice : PermanentEntity
{
    public decimal Price { get; set; }
    public bool? Available { get; set; }
    public FuelPriceStatus Status { get; set; }
    public bool Priority { get; set; }

    public long? FuelStationId { get; set; } // Should be change after changing FuelStation to permanent table
    public FuelStation? FuelStation { get; set; }

    public long? FuelTypeId { get; set; } // Should be change after changing FuelStation to permanent table
    public FuelType? FuelType { get; set; }
    
    public long UserId { get; set; }
    public User? User { get; set; }
}