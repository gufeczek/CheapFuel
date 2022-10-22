using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class FavoriteRepository : Repository<Favorite>, IFavoriteRepository
{
    public FavoriteRepository(AppDbContext context) : base(context) { }
}