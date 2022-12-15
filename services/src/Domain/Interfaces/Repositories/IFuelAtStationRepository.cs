using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IFuelAtStationRepository : IRepository<FuelAtStation>
{
    Task<FuelAtStation?> GetAsync(long fuelStationId, long fuelTypeId);
    Task<int> CountAllByFuelStationIdAndFuelTypesIdsAsync(long fuelStationId, IEnumerable<long> fuelTypesIds);
    Task<bool> ExistsAsync(long fuelStationId, long fuelTypeId);
    Task RemoveAllByFuelStationId(long fuelStationId);
}