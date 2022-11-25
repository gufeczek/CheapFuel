using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IFavoriteRepository : IRepository<Favorite>
{
    Task<Favorite?> GetByUsernameAndFuelStationIdAsync(string username, long fuelStationId);
    Task<bool> ExistsByUsernameAndFuelStationIdAsync(string username, long fuelStationId);
}