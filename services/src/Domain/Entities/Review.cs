using Domain.Common;

namespace Domain.Entities;

public class Review : PermanentEntity
{
    public int? Rate { get; set; }
    public string? Content { get; set; }
    
    public long? FuelStationId { get; set; } // Should be change after changing FuelStation to permanent table
    public FuelStation? FuelStation { get; set; }

    public long UserId { get; set; }
    public User? User { get; set; }
}