using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FuelPriceRepository : BaseRepository<FuelPrice>, IFuelPriceRepository
{
    public FuelPriceRepository(AppDbContext context) : base(context) { }

    public async Task<FuelPrice?> GetMostRecentPrice(long fuelStationId, long fuelTypeId)
    {
        return await Context.FuelPrices
            .Where(f =>
                f.Status == FuelPriceStatus.Accepted &&
                f.FuelTypeId == fuelTypeId &&
                f.FuelStationId == fuelStationId)
            .OrderByDescending(f => f.CreatedAt)
            .FirstOrDefaultAsync();
    }
}