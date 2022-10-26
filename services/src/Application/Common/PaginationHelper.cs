using Application.Common.Exceptions;
using Application.Models.Interfaces;
using Application.Models.Pagination;
using Domain.Common.Pagination.Request;

namespace Application.Common;

public static class PaginationHelper
{
    public static PageRequest<T> Eval<T>(PageRequestDto pageRequestDto, IColumnSelector<T> sortable)
        where T : class
    {
        var columnSelector = sortable.ColumnSelector;
        var sortBy = pageRequestDto.Sort?.SortBy;
        
        if (sortBy is not null && !columnSelector.ContainsKey(sortBy))
        {
            var columns = string.Join(", ", columnSelector.Keys);
            throw new BadRequestException($"Unknown column {sortBy}. You can sort by: {columns}");
        }

        SortRequest<T>? sort = null;

        if (sortBy is not null)
        {
            sort = new()
            {
                SortBy = columnSelector[sortBy],
                Direction = pageRequestDto.Sort?.SortDirection ?? SortDirection.Desc
            };
        }

        return new PageRequest<T>
        {
            PageNumber = pageRequestDto.PageNumber ?? 1,
            PageSize = pageRequestDto.PageSize ?? 10,
            Sort = sort 
        };
    }
}