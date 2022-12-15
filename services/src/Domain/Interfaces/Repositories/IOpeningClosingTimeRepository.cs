using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IOpeningClosingTimeRepository : IRepository<OpeningClosingTime>
{
    Task RemoveAllByFuelStationId(long fuelStationId);
}