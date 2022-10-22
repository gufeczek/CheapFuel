using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class ServiceRepository : BaseRepository<Service>, IServiceRepository
{
    public ServiceRepository(AppDbContext context) : base(context) { }
}