using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IOwnedStationRepository : IRepository<OwnedStation>
{
    Task<bool> ExistsByUserIdAndFuelStationIdAsync(long userId, long fuelStationId);
}