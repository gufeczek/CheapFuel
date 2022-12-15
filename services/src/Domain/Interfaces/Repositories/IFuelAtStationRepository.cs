using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IFuelAtStationRepository : IRepository<FuelAtStation>
{
    Task<int> CountAllByFuelStationIdAndFuelTypesIdsAsync(long fuelStationId, IEnumerable<long> fuelTypesIds);
    Task<bool> Exists(long fuelTypeId, long fuelStationId);
    Task RemoveAllByFuelStationId(long fuelStationId);
}