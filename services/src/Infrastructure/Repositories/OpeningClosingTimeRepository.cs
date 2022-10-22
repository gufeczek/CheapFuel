using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class OpeningClosingTimeRepository : Repository<OpeningClosingTime>, IOpeningClosingTimeRepository
{
    public OpeningClosingTimeRepository(AppDbContext context) : base(context) { }
}