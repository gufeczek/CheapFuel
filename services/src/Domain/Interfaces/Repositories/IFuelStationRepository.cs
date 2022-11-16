using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IFuelStationRepository : IBaseRepository<FuelStation>
{
    Task<FuelStation?> GetFuelStationWithAllDetails(long id);
    
    Task<IEnumerable<FuelStation>> GetFuelStationsWithFuelPrice(
        long fuelTypeId, 
        IEnumerable<long>? serviceIds,
        IEnumerable<long>? stationChainsIds, 
        decimal? minPrice, 
        decimal? maxPrice);
}