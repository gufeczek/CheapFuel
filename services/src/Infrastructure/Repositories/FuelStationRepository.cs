using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repositories;

public class FuelStationRepository : BaseRepository<FuelStation>, IFuelStationRepository
{
    public FuelStationRepository(AppDbContext context) : base(context) { }
    
    public async Task<IEnumerable<FuelStation>> GetFuelStationsWithFuelPrice(
        long fuelTypeId, 
        IEnumerable<long>? servicesIds, 
        IEnumerable<long>? stationChainsIds,
        decimal? minPrice, 
        decimal? maxPrice)
    {
        var query = Context.FuelStations
            .Include(fs => fs.StationChain)
            .Include(fs => fs.FuelPrices
                .Where(fp =>
                    fp.Available == true &&
                    fp.Status == FuelPriceStatus.Accepted &&
                    fp.FuelTypeId == fuelTypeId &&
                    (minPrice == null || minPrice <= fp.Price) &&
                    (maxPrice == null || maxPrice >= fp.Price))
                .OrderByDescending(fp => fp.CreatedAt)
                .Take(1));

        if (stationChainsIds is null && servicesIds is null)
            return await query.ToListAsync();

        if (stationChainsIds is not null && servicesIds is null)
            return await GetFuelStationsWithFuelPriceByFuelTypeIdAndStationChain(query, stationChainsIds);

        if (servicesIds is not null && stationChainsIds is null)
            return await GetFuelStationWithFuelPriceByFuelTypeIdAndServices(query, servicesIds);

        return await GetFuelStationWithFuelPriceByFuelTypeIdAndStationChainsAndServices(query, stationChainsIds!, servicesIds!);
    }

    private async Task<IEnumerable<FuelStation>> GetFuelStationsWithFuelPriceByFuelTypeIdAndStationChain(
        IIncludableQueryable<FuelStation, IEnumerable<FuelPrice>> query, 
        IEnumerable<long> stationChainIds)
    {
        return await query
            .Where(fs => 
                fs.FuelPrices.Any() &&
                stationChainIds.Contains(fs.StationChain!.Id))
            .ToListAsync();
    }

    private async Task<IEnumerable<FuelStation>> GetFuelStationWithFuelPriceByFuelTypeIdAndServices(
        IIncludableQueryable<FuelStation, IEnumerable<FuelPrice>> query, 
        IEnumerable<long> servicesIds)
    {
        return await query
            .Include(fs => fs.ServiceAtStations)
            .Where(fs => 
                fs.FuelPrices.Any() &&
                fs.ServiceAtStations.Any(ss => servicesIds.Contains(ss.ServiceId)))
            .ToListAsync();
    }
    
    private async Task<IEnumerable<FuelStation>> GetFuelStationWithFuelPriceByFuelTypeIdAndStationChainsAndServices(
        IIncludableQueryable<FuelStation, IEnumerable<FuelPrice>> query, 
        IEnumerable<long> stationChainsIds, 
        IEnumerable<long> servicesIds)
    {
        return await query
            .Where(fs => 
                fs.FuelPrices.Any() &&
                fs.ServiceAtStations.Any(ss => servicesIds.Contains(ss.ServiceId)) && 
                stationChainsIds.Contains(fs.StationChain!.Id))
            .ToListAsync();
    }

}