using Domain.Common.Interfaces;

namespace Domain.Entities;

public class OwnedStation : ICreatable
{
    public long UserId { get; set; }
    public User? User { get; set; }

    public long FuelStationId { get; set; }
    public FuelStation? FuelStation { get; set; }
    
    public long? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}