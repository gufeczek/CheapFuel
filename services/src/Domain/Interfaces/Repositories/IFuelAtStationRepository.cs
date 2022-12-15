using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IFuelAtStationRepository : IRepository<FuelAtStation>
{
    Task<int> CountAllByFuelStationIdAndFuelTypesIdsAsync(long fuelStationId, IEnumerable<long> fuelTypesIds);
    Task RemoveAllByFuelStationId(long fuelStationId);
}