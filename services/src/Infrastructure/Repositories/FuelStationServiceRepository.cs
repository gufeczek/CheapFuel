using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class FuelStationServiceRepository : BaseRepository<FuelStationService>, IFuelStationServiceRepository
{
    public FuelStationServiceRepository(AppDbContext context) : base(context) { }
}