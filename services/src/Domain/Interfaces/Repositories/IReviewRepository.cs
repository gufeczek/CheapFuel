using Domain.Common.Pagination.Request;
using Domain.Common.Pagination.Response;
using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IReviewRepository : IBaseRepository<Review>
{
    Task<Page<Review>> GetAllForFuelStationAsync(long fuelStationId, PageRequest<Review> pageRequest);
    Task<Page<Review>> GetAllForUserAsync(string username, PageRequest<Review> pageRequest);
    Task<bool> ExistsByFuelStationAndUsername(long fuelStationId, string username);
    Task<Review?> GetByFuelStationAndUsername(long fuelStationId, string username);
    Task RemoveAllByFuelStationId(long fuelStationId);
}