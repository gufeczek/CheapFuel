using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class FuelAtStationRepository : Repository<FuelAtStation>, IFuelAtStationRepository
{
    public FuelAtStationRepository(AppDbContext context) : base(context) { }
}