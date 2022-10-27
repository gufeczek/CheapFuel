using System.Linq.Expressions;

namespace Domain.Common.Pagination.Request;

public class SortRequest<TEntity> where TEntity : class
{
    public Expression<Func<TEntity, object>> SortBy { get; init; }
    public SortDirection Direction { get; init; }
}