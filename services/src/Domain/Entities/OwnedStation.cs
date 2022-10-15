using Domain.Common;

namespace Domain.Entities;

public class OwnedStation
{
    public long UserId { get; set; }
    public User? User { get; set; }

    public long FuelStationId { get; set; }
    public FuelStation? FuelStation { get; set; }
}