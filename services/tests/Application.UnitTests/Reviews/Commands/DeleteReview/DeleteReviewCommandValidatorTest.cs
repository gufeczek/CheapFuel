using Application.Reviews.Commands.DeleteReview;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Reviews.Commands.DeleteReview;

public class DeleteReviewCommandValidatorTest
{
    private readonly DeleteReviewCommandValidator _validator;

    public DeleteReviewCommandValidatorTest()
    {
        _validator = new DeleteReviewCommandValidator();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(99999999)]
    public void Validation_passes_for_correct_data(long id)
    {
        // Arrange
        var command = new DeleteReviewCommand(id);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.ReviewId);
    }
    
    [Fact]
    public void Validation_fails_for_null()
    {
        // Arrange
        var command = new DeleteReviewCommand(null);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ReviewId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-99999999)]
    public void Validation_fails_for_numbers_lesser_than_one(long id)
    {
        // Arrange
        var command = new DeleteReviewCommand(id);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ReviewId);
    }
}