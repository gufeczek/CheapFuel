using Application.FuelStationServices.Commands.CreateFuelStationService;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.FuelStationServices.Commands.CreateFuelStationService;

public class CreateFuelStationServiceCommandValidatorTest
{
    private readonly CreateFuelStationServiceCommandValidator _validator;

    public CreateFuelStationServiceCommandValidatorTest()
    {
        _validator = new CreateFuelStationServiceCommandValidator();
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("abcdef123")]
    [InlineData("12 3fdasf")]
    public void validation_passes_for_correct_data(string name)
    {
        // Arrange
        var command = new CreateFuelStationServiceCommand(name);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void validation_fails_for_empty_name(string name)
    {
        // Arrange
        var command = new CreateFuelStationServiceCommand(name);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("av")]
    [InlineData(".b")]
    public void validation_fails_for_name_shorter_then_three_characters(string name)
    {
        // Arrange
        var command = new CreateFuelStationServiceCommand(name);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }
    
    [Fact]
    public void validation_fails_for_name_with_more_than_32_characters()
    {
        // Arrange
        const string name = "kbCEBglemEMw9Q3MuMmi42CQqRBOb7djf";
        var command = new CreateFuelStationServiceCommand(name);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }
}