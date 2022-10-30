using Application.Users.Commands.AuthenticateUser;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Users.Commands.AuthenticateUser;

public class AuthenticateUserCommandValidatorTest
{
    private readonly AuthenticateUserCommandValidator _validator;

    public AuthenticateUserCommandValidatorTest()
    {
        _validator = new AuthenticateUserCommandValidator();
    }

    [Theory]
    [InlineData("Username", "Password")]
    [InlineData("1", "123")]
    [InlineData("_f3#$$", "gS123AA")]
    public void validation_passes_for_correct_data(string username, string password)
    {
        // Arrange
        var command = new AuthenticateUserCommand(username, password);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Username);
        result.ShouldNotHaveValidationErrorFor(c => c.Password);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void validation_fails_for_empty_username(string username)
    {
        // Arrange
        var command = new AuthenticateUserCommand(username, "Password");
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Username);
        result.ShouldNotHaveValidationErrorFor(c => c.Password);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void validation_fails_for_empty_password(string password)
    {
        // Arrange
        var command = new AuthenticateUserCommand("Username", password);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password);
        result.ShouldNotHaveValidationErrorFor(c => c.Username);
    }
}