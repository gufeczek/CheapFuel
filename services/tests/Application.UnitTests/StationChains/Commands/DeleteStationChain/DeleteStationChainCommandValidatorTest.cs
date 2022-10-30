using Application.StationChains.Commands.DeleteStationChain;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.StationChains.Commands.DeleteStationChain;

public class DeleteStationChainCommandValidatorTest
{
    private readonly DeleteStationChainCommandValidator _validator;

    public DeleteStationChainCommandValidatorTest()
    {
        _validator = new DeleteStationChainCommandValidator();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(99999999)]
    public void validation_passes_for_correct_data(long id)
    {
        // Arrange
        var command = new DeleteStationChainCommand(id);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Id);
    }

    [Fact]
    public void validation_fails_for_null()
    {
        // Arrange
        var command = new DeleteStationChainCommand(null);
        
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
        var command = new DeleteStationChainCommand(id);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Id);
    }
}