using Domain.Common.Pagination.Request;
using Domain.Common.Pagination.Response;
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

    public async Task<Page<Favorite>> GetAllByUsernameAsync(string username, PageRequest<Favorite> pageRequest)
    {
        var query = Context.Favorites
            .Include(f => f.User)
            .Include(f => f.FuelStation)
                .ThenInclude(fs => fs!.StationChain)
            .Where(f => f.User!.Username == username);
        return await Paginate(query, pageRequest);
    }

    public async Task<bool> ExistsByUsernameAndFuelStationIdAsync(string username, long fuelStationId)
    {
        return await Context.Favorites
            .Where(f => f.User!.Username == username && f.FuelStationId == fuelStationId)
            .AnyAsync();
    }

    public async Task RemoveAllByFuelStationId(long fuelStationId)
    {
        var toDelete = await Context.Reviews
            .Where(f => f.FuelStationId == fuelStationId)
            .ToListAsync();
        Context.Reviews.RemoveRange(toDelete);
    }
}