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

    public async Task<Page<TEntity>> GetAllAsync(PageRequest<TEntity> pageRequest)
    {
        return await Paginate(Context.Set<TEntity>(), pageRequest);
    }

    protected async Task<Page<TEntity>> Paginate(IQueryable<TEntity> query, PageRequest<TEntity> pageRequest)
    {
        if (pageRequest.Sort is not null)
        {
            query = pageRequest.Sort.Direction == SortDirection.Asc
                ? query.OrderBy(pageRequest.Sort.SortBy)
                : query.OrderByDescending(pageRequest.Sort.SortBy);
        }

        var data = await query.Skip(pageRequest.PageSize * (pageRequest.PageNumber - 1))
            .Take(pageRequest.PageSize)
            .ToListAsync();

        var totalElements = await Context.Set<TEntity>().CountAsync();
        var totalPages = (int)Math.Ceiling((decimal)totalElements / pageRequest.PageSize);

        return new Page<TEntity>
        {
            PageNumber = pageRequest.PageNumber,
            PageSize = pageRequest.PageSize,
            NextPage = pageRequest.PageNumber < totalPages ? pageRequest.PageNumber + 1 : null,
            PreviousPage = pageRequest.PageNumber > 1 ? pageRequest.PageNumber - 1 : null,
            FirstPage = 1,
            LastPage = totalPages,
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