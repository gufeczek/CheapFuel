using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OwnedStationRepository : Repository<OwnedStation>, IOwnedStationRepository
{
    public OwnedStationRepository(AppDbContext context) : base(context) { }
    
    public async Task<bool> ExistsByUserIdAndFuelStationIdAsync(long userId, long fuelStationId)
    {
        return await Context.OwnedStations
            .AnyAsync(o => o.UserId == userId && o.FuelStationId == fuelStationId);
    }

    public async Task RemoveAllByFuelStationId(long fuelStationId)
    {
        var toDelete = await Context.OwnedStations
            .Where(o => o.FuelStationId == fuelStationId)
            .ToListAsync();
        Context.OwnedStations.RemoveRange(toDelete);
    }
}