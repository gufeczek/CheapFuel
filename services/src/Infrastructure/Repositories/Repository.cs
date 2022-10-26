using System.Linq.Expressions;
using Domain.Common.Pagination;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext Context;

    protected Repository(AppDbContext context)
    {
        Context = context;
    }
    
    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await Context.Set<TEntity>()
            .Where(filter)
            .ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await Context.Set<TEntity>()
            .ToListAsync();
    }

    public async Task<Page<TEntity>> GetAllAsync(PageRequest pageRequest)
    {
        IEnumerable<TEntity> data = await Context.Set<TEntity>()
            .Skip(pageRequest.PageSize * (pageRequest.PageNumber - 1))
            .Take(pageRequest.PageSize)
            .ToListAsync();

        var totalElements = await Context.Set<TEntity>().CountAsync();
        int totalPages = (int) Math.Ceiling((decimal)totalElements / pageRequest.PageSize);

        return new Page<TEntity>
        {
            PageNumber = pageRequest.PageNumber,
            PageSize = pageRequest.PageSize,
            NextPage = pageRequest.PageNumber != totalPages ? pageRequest.PageNumber + 1 : null,
            PreviousPage = pageRequest.PageNumber != 1 ? pageRequest.PageNumber - 1 : null,
            TotalPages = totalPages,
            TotalElements = totalElements,
            Data = data
        };
    }

    public void Add(TEntity entity)
    { 
        Context.Set<TEntity>().Add(entity);
    }

    public void AddAll(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public void RemoveAll(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().RemoveRange(entities);
    }
}