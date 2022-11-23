using Application.Favorites.Commands.DeleteFavourite;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Favourites.Commands.DeleteFavourite;

public class DeleteFavouriteCommandValidatorTest
{
    private readonly DeleteFavouriteCommandValidator _validator;

    public DeleteFavouriteCommandValidatorTest()
    {
        _validator = new DeleteFavouriteCommandValidator();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(99999999)]
    public void validation_passes_for_correct_data(long id)
    {
        // Arrange
        var command = new DeleteFavouriteCommand(id);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.FuelStationId);
    }

    [Fact]
    public void validation_fails_for_null()
    {
        // Arrange
        var command = new DeleteFavouriteCommand(null);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(q => q.FuelStationId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-99999999)]
    public void validation_fails_for_numbers_lesser_than_one(long id)
    {
        // Arrange
        var command = new DeleteFavouriteCommand(id);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(q => q.FuelStationId);
    }
}