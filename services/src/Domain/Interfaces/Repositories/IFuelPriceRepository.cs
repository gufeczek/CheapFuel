using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IFuelPriceRepository : IBaseRepository<FuelPrice>
{
    public Task<FuelPrice?> GetMostRecentPrice(long fuelStationId, long fuelTypeId);
}