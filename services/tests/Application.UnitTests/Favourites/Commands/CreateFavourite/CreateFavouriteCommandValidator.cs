using Application.Favorites.Commands.CreateFavourite;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Favourites.Commands.CreateFavourite;

public class CreateFavouriteCommandValidatorTest
{
    private readonly CreateFavouriteCommandValidator _validator;

    public CreateFavouriteCommandValidatorTest()
    {
        _validator = new CreateFavouriteCommandValidator();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(99999999)]
    public void validation_passes_for_correct_data(long id)
    {
        // Arrange
        var command = new CreateFavouriteCommand(id);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.FuelStationId);
    }

    [Fact]
    public void validation_fails_for_null()
    {
        // Arrange
        var command = new CreateFavouriteCommand(null);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.FuelStationId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-99999999)]
    public void validation_fails_for_numbers_lesser_than_one(long id)
    {
        // Arrange
        var command = new CreateFavouriteCommand(id);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.FuelStationId);
    }
}