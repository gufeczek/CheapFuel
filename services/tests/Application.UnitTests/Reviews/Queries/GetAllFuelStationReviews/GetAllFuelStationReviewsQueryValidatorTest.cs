using Application.Models.Pagination;
using Application.Reviews.Queries.GetAllFuelStationReviews;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Reviews.Queries.GetAllFuelStationReviews;

public class GetAllFuelStationReviewsQueryValidatorTest
{
    private readonly GetAllFuelStationReviewsQueryValidator _validator;

    public GetAllFuelStationReviewsQueryValidatorTest()
    {
        _validator = new GetAllFuelStationReviewsQueryValidator();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(99999999)]
    public void Validation_passes_for_correct_data(long id)
    {
        // Arrange
        var pageRequest = new PageRequestDto { PageNumber = 1, PageSize = 10, Sort = null };
        var query = new GetAllFuelStationReviewsQuery(id, pageRequest);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Id);
        result.ShouldNotHaveValidationErrorFor(q => q.PageRequestDto);
    }
    
    [Fact]
    public void validation_fails_for_null_id()
    {
        // Arrange
        var pageRequest = new PageRequestDto { PageNumber = 1, PageSize = 10, Sort = null };
        var query = new GetAllFuelStationReviewsQuery(null, pageRequest);

        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Id);
        result.ShouldNotHaveValidationErrorFor(q => q.PageRequestDto);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-99999999)]
    public void validation_fails_for_numbers_lesser_than_one(long id)
    {
        // Arrange
        var pageRequest = new PageRequestDto { PageNumber = 1, PageSize = 10, Sort = null };
        var query = new GetAllFuelStationReviewsQuery(null, pageRequest);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Id);
        result.ShouldNotHaveValidationErrorFor(q => q.PageRequestDto);
    }
}