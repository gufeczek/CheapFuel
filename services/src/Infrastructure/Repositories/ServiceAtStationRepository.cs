using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class ServiceAtStationRepository : Repository<ServiceAtStation>, IServiceAtStationRepository
{
    public ServiceAtStationRepository(AppDbContext context) : base(context) { }
}