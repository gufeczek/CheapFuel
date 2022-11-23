using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FavoriteRepository : Repository<Favorite>, IFavoriteRepository
{
    public FavoriteRepository(AppDbContext context) : base(context) { }

    public async Task<Favorite?> GetByUsernameAndFuelStationIdAsync(string username, long fuelStationId)
    {
        return await Context.Favorites
            .Include(f => f.User)
            .Where(f => f.User!.Username == username && f.FuelStationId == fuelStationId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsByUsernameAndFuelStationIdAsync(string username, long fuelStationId)
    {
        return await Context.Favorites
            .Where(f => f.User!.Username == username && f.FuelStationId == fuelStationId)
            .AnyAsync();
    }
}