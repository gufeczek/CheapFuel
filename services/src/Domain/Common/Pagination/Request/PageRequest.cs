namespace Domain.Common.Pagination.Request;

public class PageRequest<TEntity> where TEntity : class
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public SortRequest<TEntity>? Sort { get; set; }
}