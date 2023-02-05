using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ServiceAtStationRepository : Repository<ServiceAtStation>, IServiceAtStationRepository
{
    public ServiceAtStationRepository(AppDbContext context) : base(context) { }

    public async Task<ServiceAtStation?> GetAsync(long fuelStationId, long serviceId)
    {
        return await Context.ServiceAtStations
            .Where(sas => sas.FuelStationId == fuelStationId && sas.ServiceId == serviceId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsAsync(long fuelStationId, long serviceId)
    {
        return await Context.ServiceAtStations
            .Where(sas => sas.FuelStationId == fuelStationId && sas.ServiceId == serviceId)
            .AnyAsync();
    }

    public async Task RemoveAllByFuelStationId(long fuelStationId)
    {
        var toDelete = await Context.ServiceAtStations
            .Where(sas => sas.FuelStationId == fuelStationId)
            .ToListAsync();
        Context.ServiceAtStations.RemoveRange(toDelete);
    }
}