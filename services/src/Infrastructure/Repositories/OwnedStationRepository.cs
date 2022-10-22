using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class OwnedStationRepository : Repository<OwnedStation>, IOwnedStationRepository
{
    public OwnedStationRepository(AppDbContext context) : base(context) { }
}