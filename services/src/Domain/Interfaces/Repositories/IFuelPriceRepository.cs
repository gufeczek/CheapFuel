using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IFuelPriceRepository : IBaseRepository<FuelPrice>
{
    Task<FuelPrice?> GetMostRecentPrice(long fuelStationId, long fuelTypeId);
    Task RemoveAllByFuelStationId(long fuelStationId);
}