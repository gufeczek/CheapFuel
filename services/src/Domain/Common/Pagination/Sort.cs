using System.Linq.Expressions;

namespace Domain.Common.Pagination;

public class Sort<TEntity> where TEntity : class
{
    public Expression<Func<TEntity, object>> SortBy { get; init; }
    public SortDirection Direction;
}