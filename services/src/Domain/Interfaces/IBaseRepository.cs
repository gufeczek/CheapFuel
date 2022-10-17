using Domain.Common;

namespace Domain.Interfaces;

public interface IBaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> Get(long id);
}