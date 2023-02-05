using System.Linq.Expressions;
using Domain.Common.Pagination.Request;
using Domain.Common.Pagination.Response;

namespace Domain.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<Page<TEntity>> GetAllAsync(PageRequest<TEntity> pageRequest);

    void Add(TEntity entity);
    void AddAll(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);
    void RemoveAll(IEnumerable<TEntity> entities);
}