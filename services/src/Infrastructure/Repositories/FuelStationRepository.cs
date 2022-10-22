using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class FuelStationRepository : BaseRepository<FuelStation>, IFuelStationRepository
{
    public FuelStationRepository(AppDbContext context) : base(context) { }
}