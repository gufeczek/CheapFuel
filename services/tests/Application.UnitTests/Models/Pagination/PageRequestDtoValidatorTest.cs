using System.Collections.Generic;
using Application.Models.Pagination;
using Domain.Common.Pagination.Request;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Models.Pagination;

public class PageRequestDtoValidatorTest
{
    private readonly PageRequestDtoValidator _validator;

    public PageRequestDtoValidatorTest()
    {
        _validator = new PageRequestDtoValidator();
    }

    [Theory]
    [MemberData(nameof(CorrectData))]
    public void validation_passes_for_correct_data(PageRequestDto pageRequest)
    {
        // Act
        var result = _validator.TestValidate(pageRequest);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(p => p.PageNumber);
        result.ShouldNotHaveValidationErrorFor(p => p.PageSize);
        result.ShouldNotHaveValidationErrorFor(p => p.Sort);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999999)]
    public void validation_fails_for_page_number_lesser_than_one(int pageNumber)
    {
        // Arrange
        var pageRequest = new PageRequestDto { PageNumber = pageNumber, PageSize = 10, Sort = null };

        // Act
        var result = _validator.TestValidate(pageRequest);
        
        // Assert
        result.ShouldHaveValidationErrorFor(p => p.PageNumber);
        result.ShouldNotHaveValidationErrorFor(p => p.PageSize);
        result.ShouldNotHaveValidationErrorFor(p => p.Sort);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999999)]
    public void validation_fails_for_page_size_lesser_than_one(int pageSize)
    {
        // Arrange
        var pageRequest = new PageRequestDto { PageNumber = 1, PageSize = pageSize, Sort = null };

        // Act
        var result = _validator.TestValidate(pageRequest);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(p => p.PageNumber);
        result.ShouldHaveValidationErrorFor(p => p.PageSize);
        result.ShouldNotHaveValidationErrorFor(p => p.Sort);
    }

    public static IEnumerable<object[]> CorrectData => new List<object[]>
    {
        new object[]
        {
            new PageRequestDto
            {
                PageNumber = 1, 
                PageSize = 1, 
                Sort = new SortDto { SortBy = "Name", SortDirection = SortDirection.Desc }
            }
        },
        new object[]
        {
            new PageRequestDto
            {
                PageNumber = 1, 
                PageSize = 10, 
                Sort = new SortDto { SortBy = "Name", SortDirection = SortDirection.Desc }
            }
        },
        new object[]
        {
            new PageRequestDto
            {
                PageNumber = 1, 
                PageSize = 10, 
                Sort = new SortDto { SortBy = null, SortDirection = SortDirection.Desc }
            }
        },
        new object[]
        {
            new PageRequestDto
            {
                PageNumber = 1, 
                PageSize = 10, 
                Sort = new SortDto { SortBy = "Name", SortDirection = null }
            }
        },
        new object[]
        {
            new PageRequestDto
            {
                PageNumber = 1, 
                PageSize = 10, 
                Sort = null
            }
        },
        new object[]
        {
            new PageRequestDto
            {
                PageNumber = null, 
                PageSize = 10, 
                Sort = new SortDto { SortBy = "Name", SortDirection = SortDirection.Desc }
            }
        },
        new object[]
        {
            new PageRequestDto
            {
                PageNumber = 1, 
                PageSize = null, 
                Sort = new SortDto { SortBy = "Name", SortDirection = SortDirection.Desc }
            }
        }
    };
}