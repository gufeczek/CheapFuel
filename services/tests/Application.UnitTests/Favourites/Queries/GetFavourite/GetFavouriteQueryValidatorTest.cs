using Application.Favorites.Queries.GetFavourite;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Favourites.Queries.GetFavourite;

public class GetFavouriteQueryValidatorTest
{
    private readonly GetFavouriteQueryValidator _validator;

    public GetFavouriteQueryValidatorTest()
    {
        _validator = new GetFavouriteQueryValidator();
    }
    
    [Theory]
    [InlineData("a", 1)]
    [InlineData("User", 10)]
    [InlineData("1234", 99999999)]
    public void validation_passes_for_correct_data(string username, long fuelStationId)
    {
        // Arrange
        var command = new GetFavouriteQuery(username, fuelStationId);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Username);
        result.ShouldNotHaveValidationErrorFor(q => q.FuelStationId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void validation_fails_for_empty_username(string username)
    {
        // Arrange
        var query = new GetFavouriteQuery(username, 1);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Username);
        result.ShouldNotHaveValidationErrorFor(q => q.FuelStationId);
    }

    [Fact]
    public void validation_fails_for_null_fuel_station_id()
    {
        // Arrange
        var command = new GetFavouriteQuery("User", null);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Username);
        result.ShouldHaveValidationErrorFor(q => q.FuelStationId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-99999999)]
    public void validation_fails_for_fuel_station_id_smaller_than_one(long fuelStationId)
    {
        // Arrange
        var command = new GetFavouriteQuery("User", fuelStationId);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Username);
        result.ShouldHaveValidationErrorFor(q => q.FuelStationId);
    }
}