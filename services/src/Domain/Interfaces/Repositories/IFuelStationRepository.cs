using Domain.Common.Pagination.Request;
using Domain.Common.Pagination.Response;
using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IFuelStationRepository : IBaseRepository<FuelStation>
{
    Task<FuelStation?> GetFuelStationWithAllDetailsAsync(long id);
    Task<FuelStation?> GetFuelStationWithFuelTypesAsync(long id);
    Task<IEnumerable<FuelStation>> GetFuelStationWithPricesAsync(long fuelTypeId);
    public Task<Page<FuelStation>> GetFuelStationsWithPrices(
        long fuelTypeId,
        IEnumerable<long>? servicesIds,
        IEnumerable<long>? stationChainsIds,
        decimal? minPrice,
        decimal? maxPrice,
        PageRequest<FuelStation> pageRequest);
    
    Task<IEnumerable<FuelStation>> GetFuelStationsWithFuelPrice(
        long fuelTypeId, 
        IEnumerable<long>? serviceIds,
        IEnumerable<long>? stationChainsIds, 
        decimal? minPrice, 
        decimal? maxPrice);
}