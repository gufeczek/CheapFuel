
namespace Application.Models.Pagination;

public sealed class PageRequestDto
{
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; } = 10;
    public SortDto? Sort { get; init; }
}