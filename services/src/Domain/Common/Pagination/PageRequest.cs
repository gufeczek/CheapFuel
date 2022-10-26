namespace Domain.Common.Pagination;

public class PageRequest<TEntity> where TEntity : class
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public Sort<TEntity>? Sort { get; set; }
}