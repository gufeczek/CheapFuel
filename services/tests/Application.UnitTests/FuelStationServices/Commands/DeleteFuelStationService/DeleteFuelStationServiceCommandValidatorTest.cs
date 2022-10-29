using Application.FuelStationServices.Commands.DeleteFuelStationService;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.FuelStationServices.Commands.DeleteFuelStationService;

public class DeleteFuelStationServiceCommandValidatorTest
{
    private readonly DeleteFuelStationServiceCommandValidator _validator;

    public DeleteFuelStationServiceCommandValidatorTest()
    {
        _validator = new DeleteFuelStationServiceCommandValidator();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(99999999)]
    public void validation_passes_for_correct_data(long id)
    {
        // Arrange
        var command = new DeleteFuelStationServiceCommand(id);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Id);
    }

    [Fact]
    public void validation_fails_for_null()
    {
        // Arrange
        var command = new DeleteFuelStationServiceCommand(null);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Id);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-99999999)]
    public void validation_fails_for_numbers_lesser_than_one(long id)
    {
        // Arrange
        var command = new DeleteFuelStationServiceCommand(id);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Id);
    }
}