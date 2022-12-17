using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IServiceAtStationRepository : IRepository<ServiceAtStation>
{
    Task<ServiceAtStation?> GetAsync(long fuelStationId, long serviceId);
    Task<bool> ExistsAsync(long fuelStationId, long serviceId);
    Task RemoveAllByFuelStationId(long fuelStationId);
}