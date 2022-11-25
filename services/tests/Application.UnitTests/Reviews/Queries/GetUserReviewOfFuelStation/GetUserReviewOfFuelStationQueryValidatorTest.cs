using Application.Reviews.Queries.GetUserReviewOfFuelStation;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Reviews.Queries.GetUserReviewOfFuelStation;

public class GetUserReviewOfFuelStationQueryValidatorTest
{
    private readonly GetUserReviewOfFuelStationQueryValidator _validator;

    public GetUserReviewOfFuelStationQueryValidatorTest()
    {
        _validator = new GetUserReviewOfFuelStationQueryValidator();
    }

    [Theory]
    [InlineData(1, "User")]
    [InlineData(1, "f")]
    [InlineData(99999999, "_f3#$$")]
    public void Validation_passes_for_correct_data(long fuelStationId, string username)
    {
        // Arrange
        var query = new GetUserReviewOfFuelStationQuery(username, fuelStationId);
        
        // Act
        var result = _validator.TestValidate(query);
        
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
        var query = new GetUserReviewOfFuelStationQuery(username, 1);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Username);
        result.ShouldNotHaveValidationErrorFor(q => q.FuelStationId);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-99999999)]
    public void Validation_fails_for_fuel_station_id_smaller_than_one(long fuelStationId)
    {
        // Arrange
        var query = new GetUserReviewOfFuelStationQuery("User", fuelStationId);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Username);
        result.ShouldHaveValidationErrorFor(q => q.FuelStationId);
    }

    [Fact]
    public void Validation_fails_for_null_fuel_station_id()
    {
        // Arrange
        var query = new GetUserReviewOfFuelStationQuery("User", null);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Username);
        result.ShouldHaveValidationErrorFor(q => q.FuelStationId);
    }
}