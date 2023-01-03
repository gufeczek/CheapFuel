using Domain.Common.Pagination.Request;
using Domain.Common.Pagination.Response;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FuelStationRepository : BaseRepository<FuelStation>, IFuelStationRepository
{
    public FuelStationRepository(AppDbContext context) : base(context) { }

    public async Task<FuelStation?> GetFuelStationWithAllDetailsAsync(long id)
    {
        return await Context.FuelStations
            .Include(fs => fs.StationChain)
            .Include(fs => fs.OpeningClosingTimes)
            .Include(fs => fs.ServiceAtStations)
                .ThenInclude(ss => ss.Service)
            .Include(fs => fs.FuelTypes)
                .ThenInclude(ft => ft.FuelType)
            .Include(fs => fs.OwnedStations)
                .ThenInclude(os => os.User)
            .Where(fs => fs.Id == id)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<FuelStation>> GetFuelStationWithPricesAsync(long fuelTypeId)
    {
        var query = Context.FuelStations
            .Include(fs => fs.StationChain)
            .Include(fs => fs.FuelPrices
                .Where(fp =>
                    fp.Status == FuelPriceStatus.Accepted &&
                    fp.FuelTypeId == fuelTypeId &&
                    fp.Available == true)
                .OrderByDescending(fp => fp.CreatedAt)
                .Take(1))
            .Where(fs => 
                fs.FuelPrices.Any(fp => fp.Status == FuelPriceStatus.Accepted &&
                                        fp.FuelTypeId == fuelTypeId &&
                                        fp.Available == true
                                        && fs.FuelTypes.Any(ft => ft.FuelTypeId == fuelTypeId)));

        return await query.ToListAsync();
    }

    public async Task<Page<FuelStation>> GetFuelStationsWithPrices(
        long fuelTypeId,
        IEnumerable<long>? servicesIds, 
        IEnumerable<long>? stationChainsIds,
        decimal? minPrice, 
        decimal? maxPrice,
        PageRequest<FuelStation> pageRequest)
    {
        var query = GetQueryForFuelStationsWithFuelPrice(
            fuelTypeId, 
            servicesIds, 
            stationChainsIds, 
            minPrice, 
            maxPrice);

        return await Paginate(query, pageRequest);
    }

    public async Task<FuelStation?> GetFuelStationWithFuelTypesAsync(long id)
    {
        return await Context.FuelStations
            .Include(fs => fs.FuelTypes)
            .Where(fs => fs.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<FuelStation>> GetFuelStationsWithFuelPrice(
        long fuelTypeId, 
        IEnumerable<long>? servicesIds, 
        IEnumerable<long>? stationChainsIds,
        decimal? minPrice, 
        decimal? maxPrice)
    {
        return await GetQueryForFuelStationsWithFuelPrice(
            fuelTypeId, 
            servicesIds, 
            stationChainsIds, 
            minPrice, 
            maxPrice).ToListAsync();
    }

    private IQueryable<FuelStation> GetQueryForFuelStationsWithFuelPrice(long fuelTypeId,
        IEnumerable<long>? servicesIds,
        IEnumerable<long>? stationChainsIds,
        decimal? minPrice,
        decimal? maxPrice)
    {
        var query = Context.FuelStations
            .Include(fs => fs.StationChain)
            .Include(fs => fs.FuelPrices
                .Where(fp =>
                    fp.Status == FuelPriceStatus.Accepted &&
                    fp.FuelTypeId == fuelTypeId &&
                    fp.Available == true)
                .OrderByDescending(fp => fp.CreatedAt)
                .Take(1))
            .Where(fs => 
                fs.FuelPrices.Any(fp => fp.Status == FuelPriceStatus.Accepted &&
                                        fp.FuelTypeId == fuelTypeId &&
                                        fp.Available == true
                && fs.FuelTypes.Any(ft => ft.FuelTypeId == fuelTypeId)));

        if (stationChainsIds is null && servicesIds is null)
            return query;

        if (stationChainsIds is not null && servicesIds is null)
            return GetQueryForFuelStationsWithFuelPriceByFuelTypeIdAndStationChain(query, stationChainsIds);

        if (servicesIds is not null && stationChainsIds is null)
            return GetQueryForFuelStationWithFuelPriceByFuelTypeIdAndServices(query, servicesIds);

        return GetQueryForFuelStationWithFuelPriceByFuelTypeIdAndStationChainsAndServices(query, stationChainsIds!, servicesIds!);
    }

    private IQueryable<FuelStation> GetQueryForFuelStationsWithFuelPriceByFuelTypeIdAndStationChain(
        IQueryable<FuelStation> query,
        IEnumerable<long> stationChainIds)
    {
        return query
            .Where(fs => stationChainIds.Contains(fs.StationChain!.Id));
    }

    private IQueryable<FuelStation> GetQueryForFuelStationWithFuelPriceByFuelTypeIdAndServices(
        IQueryable<FuelStation> query, 
        IEnumerable<long> servicesIds)
    {
        return query
            .Include(fs => fs.ServiceAtStations)
            .Where(fs => fs.ServiceAtStations.Any(ss => servicesIds.Contains(ss.ServiceId)));
    }
    
    private IQueryable<FuelStation> GetQueryForFuelStationWithFuelPriceByFuelTypeIdAndStationChainsAndServices(
        IQueryable<FuelStation> query, 
        IEnumerable<long> stationChainsIds, 
        IEnumerable<long> servicesIds)
    {
        return query
            .Where(fs =>
                fs.ServiceAtStations.Any(ss => servicesIds.Contains(ss.ServiceId)) && 
                stationChainsIds.Contains(fs.StationChain!.Id));
    }
}