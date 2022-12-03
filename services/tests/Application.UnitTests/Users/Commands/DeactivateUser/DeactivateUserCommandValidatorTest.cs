using Application.Users.Commands.DeactivateUser;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Users.Commands.DeactivateUser;

public class DeactivateUserCommandValidatorTest
{
    private readonly DeactivateUserCommandValidator _validator;
    
    public DeactivateUserCommandValidatorTest()
    {
        _validator = new DeactivateUserCommandValidator();
    }
    
    [Theory]
    [InlineData("User")]
    [InlineData("a")]
    [InlineData("1234567")]
    public void Validation_passes_for_correct_data(string username)
    {
        // Arrange
        var command = new DeactivateUserCommand(username);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Username);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Validation_fails_for_empty_username(string username)
    {
        // Arrange
        var command = new DeactivateUserCommand(username);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Username);
    }
}