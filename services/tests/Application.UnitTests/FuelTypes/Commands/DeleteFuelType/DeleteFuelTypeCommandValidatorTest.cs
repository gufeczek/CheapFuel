using Application.FuelTypes.Commands.DeleteFuelType;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.FuelTypes.Commands.DeleteFuelType;

public class DeleteFuelTypeCommandValidatorTest
{
    private readonly DeleteFuelTypeCommandValidator _validator;

    public DeleteFuelTypeCommandValidatorTest()
    {
        _validator = new DeleteFuelTypeCommandValidator();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(99999999)]
    public void validation_passes_for_correct_data(long id)
    {
        // Arrange
        var command = new DeleteFuelTypeCommand(id);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Id);
    }

    [Fact]
    public void validation_fails_for_null()
    {
        // Arrange
        var command = new DeleteFuelTypeCommand(null);
        
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
        var command = new DeleteFuelTypeCommand(id);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Id);
    }
}