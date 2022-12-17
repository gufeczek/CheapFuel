using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FuelAtStationRepository : Repository<FuelAtStation>, IFuelAtStationRepository
{
    public FuelAtStationRepository(AppDbContext context) : base(context) { }

    public async Task<FuelAtStation?> GetAsync(long fuelStationId, long fuelTypeId)
    {
        return await Context.FuelAtStations
            .Where(fas => fas.FuelStationId == fuelStationId && fas.FuelTypeId == fuelTypeId)
            .FirstOrDefaultAsync();
    }

    public async Task<int> CountAllByFuelStationIdAndFuelTypesIdsAsync(long fuelStationId, IEnumerable<long> fuelTypesIds)
    {
        return await Context.FuelAtStations
            .Where(fas => fas.FuelStationId == fuelStationId && fuelTypesIds.Contains(fas.FuelTypeId))
            .CountAsync();
    }

    public async Task<bool> ExistsAsync(long fuelStationId, long fuelTypeId)
    {
        return await Context.FuelAtStations
            .Where(fas => fas.FuelStationId == fuelStationId && fas.FuelTypeId == fuelTypeId)
            .AnyAsync();
    }

    public async Task RemoveAllByFuelStationId(long fuelStationId)
    {
        var toDelete = await Context.FuelAtStations
            .Where(fas => fas.FuelStationId == fuelStationId)
            .ToListAsync();
        Context.FuelAtStations.RemoveRange(toDelete);
    }
}