using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class StationChainRepository : BaseRepository<StationChain>, IStationChainRepository
{
    public StationChainRepository(AppDbContext context) : base(context) { }
}