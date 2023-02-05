using Domain.Common.Pagination.Request;
using Domain.Common.Pagination.Response;
using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IFavoriteRepository : IRepository<Favorite>
{
    Task<Favorite?> GetByUsernameAndFuelStationIdAsync(string username, long fuelStationId);
    Task<Page<Favorite>> GetAllByUsernameAsync(string username, PageRequest<Favorite> pageRequest);
    Task<bool> ExistsByUsernameAndFuelStationIdAsync(string username, long fuelStationId);
    Task RemoveAllByFuelStationId(long fuelStationId);
}