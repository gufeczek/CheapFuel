namespace Domain.Common.Pagination.Response;

public sealed class Page<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int? NextPage { get; set; }
    public int? PreviousPage { get; set; }
    public int FirstPage { get; set; }
    public int LastPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalElements { get; set; }
    public Sort? Sort { get; set; }
    public IEnumerable<T> Data { get; set; }

    public static Page<T> From<E>(Page<E> page, IEnumerable<T> data)
    {
        return new Page<T>
        {
            PageNumber = page.PageNumber,
            PageSize = page.PageSize,
            NextPage = page.NextPage,
            PreviousPage = page.PreviousPage,
            FirstPage = page.FirstPage,
            LastPage = page.LastPage,
            TotalPages = page.TotalPages,
            TotalElements = page.TotalElements,
            Sort = page.Sort,
            Data = data
        };
    }
}