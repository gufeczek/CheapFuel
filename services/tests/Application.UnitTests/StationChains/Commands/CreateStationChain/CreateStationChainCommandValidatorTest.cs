using Application.StationChains.Commands.CreateStationChain;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.StationChains.Commands.CreateStationChain;

public class CreateStationChainCommandValidatorTest
{
    private readonly CreateStationChainCommandValidator _validator;

    public CreateStationChainCommandValidatorTest()
    {
        _validator = new CreateStationChainCommandValidator();
    }

    [Theory]
    [InlineData("ab")]
    [InlineData("abcdef123")]
    [InlineData("12 3fdasf")]
    public void validation_passes_for_correct_data(string name)
    {
        // Arrange
        var command = new CreateStationChainCommand(name);
        
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
        var command = new CreateStationChainCommand(name);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Theory]
    [InlineData("a")]
    [InlineData(".")]
    public void validation_fails_for_name_shorter_then_two_characters(string name)
    {
        // Arrange
        var command = new CreateStationChainCommand(name);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }
    
    [Fact]
    public void validation_fails_for_name_with_more_than_128_characters()
    {
        // Arrange
        const string name = "D3q50PghSSmRjHBiemmBwWcmYAayabEw7Hj78UMw1ymZneI0sW31HLUwzyV1TccC" +
                            "Bje51sCsjfoi4NwBpsX784KApeSUMmqNiS49SdgRpTuOvJisui3ca94Y8ZWeEcNWr";
        var command = new CreateStationChainCommand(name);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }
}