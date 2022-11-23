using Application.Users.Commands.VerifyEmail;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Users.Commands.VerifyEmail;

public class VerifyEmailCommandValidatorTest
{
    private readonly VerifyEmailCommandValidator _validator;

    public VerifyEmailCommandValidatorTest()
    {
        _validator = new VerifyEmailCommandValidator();
    }

    [Theory]
    [InlineData("ABCDEF")]
    [InlineData("123456")]
    [InlineData("fs24fb")]
    public void Validation_passes_for_correct_data(string token)
    {
        // Arrange
        var command = new VerifyEmailCommand(token);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Token);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("ABCDE")]
    [InlineData("1234567")]
    public void Validation_fails_if_token_has_different_lenght_than_six(string token)
    {
        // Arrange
        var command = new VerifyEmailCommand(token);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Token);
    }
}