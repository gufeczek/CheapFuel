using Application.Users.Commands.GeneratePasswordResetToken;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Users.Commands.GeneratePasswordResetToken;

public class GeneratePasswordResetTokenCommandValidatorTest
{
    private readonly GeneratePasswordResetTokenCommandValidator _validator;

    public GeneratePasswordResetTokenCommandValidatorTest()
    {
        _validator = new GeneratePasswordResetTokenCommandValidator();
    }
    
    [Theory]
    [InlineData("username@email.com")]
    [InlineData("123abc@gmail.com")]
    [InlineData("abc.def@mail-archive.com")]
    public void validation_passes_for_correct_data(string email)
    {
        // Arrange
        var command = new GeneratePasswordResetTokenCommand(email);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("abc")]
    public void validation_fails_for_invalid_email(string email)
    {
        // Arrange
        var command = new GeneratePasswordResetTokenCommand(email);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }
}