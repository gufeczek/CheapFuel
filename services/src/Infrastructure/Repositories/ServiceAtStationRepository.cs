using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ServiceAtStationRepository : Repository<ServiceAtStation>, IServiceAtStationRepository
{
    public ServiceAtStationRepository(AppDbContext context) : base(context) { }

    public async Task RemoveAllByFuelStationId(long fuelStationId)
    {
        var toDelete = await Context.ServiceAtStations
            .Where(sas => sas.FuelStationId == fuelStationId)
            .ToListAsync();
        Context.ServiceAtStations.RemoveRange(toDelete);
    }
}