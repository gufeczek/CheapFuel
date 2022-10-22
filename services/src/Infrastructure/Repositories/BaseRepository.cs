using Domain.Common;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> : Repository<TEntity>, IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected BaseRepository(AppDbContext context) : base(context) { }
    
    public async Task<TEntity?> GetAsync(long id)
    {
        return await Context.Set<TEntity>()
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync();
    }
}