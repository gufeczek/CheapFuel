using Domain.Common.Pagination;

namespace Application.Models.Pagination;

public sealed class SortDto
{
    public string? SortBy { get; init; }
    public SortDirection? SortDirection { get; init; }
}