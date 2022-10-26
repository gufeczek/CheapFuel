namespace Domain.Common.Pagination;

public sealed class Page<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int? NextPage { get; set; }
    public int? PreviousPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalElements { get; set; }
    public IEnumerable<T> Data { get; set; }

    public static Page<T> From<E>(Page<E> page, IEnumerable<T> data)
    {
        return new Page<T>
        {
            PageNumber = page.PageNumber,
            PageSize = page.PageSize,
            NextPage = page.NextPage,
            PreviousPage = page.PreviousPage,
            TotalPages = page.TotalPages,
            TotalElements = page.TotalElements,
            Data = data
        };
    }
}