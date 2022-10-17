using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> filter);
    Task<IEnumerable<TEntity>> GetAll();

    void Add(TEntity entity);
    void AddAll(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);
    void RemoveAll(IEnumerable<TEntity> entities);
}