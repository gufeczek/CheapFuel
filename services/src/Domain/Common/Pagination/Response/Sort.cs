using Domain.Common.Pagination.Request;

namespace Domain.Common.Pagination.Response;

public class Sort
{
    public string? SortBy { get; init; }
    public SortDirection? Direction { get; init; }
}