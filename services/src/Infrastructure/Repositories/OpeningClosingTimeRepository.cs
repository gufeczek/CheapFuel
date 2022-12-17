using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OpeningClosingTimeRepository : Repository<OpeningClosingTime>, IOpeningClosingTimeRepository
{
    public OpeningClosingTimeRepository(AppDbContext context) : base(context) { }

    public async Task RemoveAllByFuelStationId(long fuelStationId)
    {
        var toDelete = await Context.OpeningClosingTimes
            .Where(oct => oct.FuelStationId == fuelStationId)
            .ToListAsync();
        Context.OpeningClosingTimes.RemoveRange(toDelete);
    }
}