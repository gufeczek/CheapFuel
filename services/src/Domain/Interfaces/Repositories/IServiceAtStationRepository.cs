using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IServiceAtStationRepository : IRepository<ServiceAtStation>
{
    Task RemoveAllByFuelStationId(long fuelStationId);
}