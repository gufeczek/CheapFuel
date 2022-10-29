using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Application.Common;
using Application.Common.Exceptions;
using Application.Models.Interfaces;
using Application.Models.Pagination;
using Domain.Common;
using Domain.Common.Pagination.Request;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.Common;

public class PaginationHelperTest
{
    [Fact]
    public void Evaluating_pageRequestDto_should_return_valid_pageRequest()
    {
        // Arrange
        var pageRequest = new PageRequestDto
        {
            PageSize = 10,
            PageNumber = 1,
            Sort = new SortDto { SortBy = "Id", SortDirection = SortDirection.Asc }
        };
        Expression<Func<BaseEntity, object>> expectedFunc = r => r.Id;
        var columnSelectorMock = MockColumnSelector(nameof(BaseEntity.Id), expectedFunc);

        // Act
        var result = PaginationHelper.Eval(pageRequest, columnSelectorMock.Object);
        
        // Assert
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.Sort?.SortBy.ToString().Should().Be(expectedFunc.ToString());
        result.Sort?.Direction.Should().Be(SortDirection.Asc);
    }

    [Fact]
    public void Evaluating_pageRequestDto_should_return_page_request_with_null_sort_when_sort_is_not_specified()
    {
        // Arrange
        var pageRequest = new PageRequestDto
        {
            PageSize = 10,
            PageNumber = 1,
            Sort = null
        };
        Expression<Func<BaseEntity, object>> expectedFunc = r => r.Id;
        var columnSelectorMock = MockColumnSelector(nameof(BaseEntity.Id), expectedFunc);

        // Act
        var result = PaginationHelper.Eval(pageRequest, columnSelectorMock.Object);
        
        // Assert
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.Sort.Should().BeNull();
    }

    [Fact]
    public void Evaluating_pageRequestDto_should_return_page_request_with_null_sort_when_not_specified_sorting_column()
    {
        // Arrange
        var pageRequest = new PageRequestDto
        {
            PageSize = 10,
            PageNumber = 1,
            Sort = new SortDto { SortBy = null, SortDirection = SortDirection.Asc }
        };
        Expression<Func<BaseEntity, object>> expectedFunc = r => r.Id;
        var columnSelectorMock = MockColumnSelector(nameof(BaseEntity.Id), expectedFunc);

        // Act
        var result = PaginationHelper.Eval(pageRequest, columnSelectorMock.Object);
        
        // Assert
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.Sort.Should().BeNull();
    }
    
    [Fact]
    public void Evaluating_pageRequestDto_should_return_page_request_with_sort_with_descending_direction_when_direction_is_not_specified()
    {
        // Arrange
        var pageRequest = new PageRequestDto
        {
            PageSize = 10,
            PageNumber = 1,
            Sort = new SortDto { SortBy = "Id", SortDirection = null }
        };
        Expression<Func<BaseEntity, object>> expectedFunc = r => r.Id;
        var columnSelectorMock = MockColumnSelector(nameof(BaseEntity.Id), expectedFunc);

        // Act
        var result = PaginationHelper.Eval(pageRequest, columnSelectorMock.Object);
        
        // Assert
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.Sort?.SortBy.ToString().Should().Be(expectedFunc.ToString());
        result.Sort?.Direction.Should().Be(SortDirection.Desc);
    } 
    
    [Fact]
    public void Evaluating_pageRequestDto_should_throw_exception_for_wrong_column_name()
    {
        // Arrange
        var pageRequest = new PageRequestDto
        {
            PageSize = 10,
            PageNumber = 1,
            Sort = new SortDto { SortBy = "Name", SortDirection = SortDirection.Asc }
        };
        Expression<Func<BaseEntity, object>> expectedFunc = r => r.Id;
        var columnSelectorMock = MockColumnSelector(nameof(BaseEntity.Id), expectedFunc);

        // Act
        Action act = () => PaginationHelper.Eval(pageRequest, columnSelectorMock.Object);
        
        // Assert
        act.Should()
            .Throw<BadRequestException>()
            .WithMessage("Unknown column Name. You can sort by: Id");
    }

    private Mock<IColumnSelector<BaseEntity>> MockColumnSelector(string name, Expression<Func<BaseEntity, object>> func)
    {
        var columnSelectorMock = new Mock<IColumnSelector<BaseEntity>>();
        columnSelectorMock
            .Setup(x => x.ColumnSelector)
            .Returns(new Dictionary<string, Expression<Func<BaseEntity, object>>> { { name, func } });
        return columnSelectorMock;
    }
}