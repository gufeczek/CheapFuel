using Application.Users.Commands.ResetPassword;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Users.Commands.ResetPassword;

public class ResetPasswordCommandValidatorTest
{
    private readonly ResetPasswordCommandValidator _validator;

    public ResetPasswordCommandValidatorTest()
    {
        _validator = new ResetPasswordCommandValidator();
    }

    [Theory]
    [InlineData("ABCDEF", "username@email.com", "Password1")]
    [InlineData("AAAAAA", "123abc@gmail.com", "@fhA123|123")]
    [InlineData("123456", "abc.def@mail-archive.com", "Fab123Fgaa")]
    public void validation_passes_for_correct_data(string token, string email, string newPassword)
    {
        // Arrange
        var command = new ResetPasswordCommand(email, token, newPassword, newPassword);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Token);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
        result.ShouldNotHaveValidationErrorFor(c => c.NewPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.ConfirmNewPassword);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("abc")]
    public void validation_fails_for_invalid_email(string email)
    {
        // Arrange
        var command = new ResetPasswordCommand(email, "ABCDEF", "Password123", "Password123");
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Token);
        result.ShouldHaveValidationErrorFor(c => c.Email);
        result.ShouldNotHaveValidationErrorFor(c => c.NewPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.ConfirmNewPassword);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("A")]
    [InlineData("1234")]
    [InlineData("      ")]
    [InlineData("ABCDEFGH")]
    public void validation_fails_for_invalid_token(string token)
    {
        // Arrange 
        var command = new ResetPasswordCommand("email@gmail.com", token, "Password123", "Password123");
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Token);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
        result.ShouldNotHaveValidationErrorFor(c => c.NewPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.ConfirmNewPassword);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void validation_fails_for_empty_new_passwords(string newPassword)
    {
        // Arrange
        var command = new ResetPasswordCommand("email@gmail.com", "ABCDEF", newPassword, newPassword);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Token);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
        result.ShouldHaveValidationErrorFor(c => c.NewPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.ConfirmNewPassword);
    }
    
    [Theory]
    [InlineData("a")]
    [InlineData("aB1")]
    [InlineData("abcDEF1")]
    public void validation_fails_for_new_passwords_shorter_then_eight_characters(string newPassword)
    {
        // Arrange
        var command = new ResetPasswordCommand("email@gmail.com", "ABCDEF", newPassword, newPassword);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Token);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
        result.ShouldHaveValidationErrorFor(c => c.NewPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.ConfirmNewPassword);
    }
    
    [Fact]
    public void validation_fails_for_new_passwords_with_more_than_32_characters()
    {
        // Arrange
        const string newPassword = "kbCEBglemEMw9Q3MuMmi42CQqRBOb7djf";
        var command = new ResetPasswordCommand("email@gmail.com", "ABCDEF", newPassword, newPassword);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Token);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
        result.ShouldHaveValidationErrorFor(c => c.NewPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.ConfirmNewPassword);
    }
    
    [Theory]
    [InlineData("abcdefghti")]
    [InlineData("ABCDEFGHIFDS")]
    [InlineData("123456789")]
    [InlineData("abcdeABCD")]
    [InlineData("1234advfds")]
    [InlineData("AFFDS1234")]
    public void validation_fails_for_new_passwords_without_at_least_one_lowercase_and_uppercase_letter_and_one_digit(string newPassword)
    {
        // Arrange
        var command = new ResetPasswordCommand("email@gmail.com", "ABCDEF", newPassword, newPassword);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Token);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
        result.ShouldHaveValidationErrorFor(c => c.NewPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.ConfirmNewPassword);
    }
    
    [Theory]
    [InlineData("Bożydar1234")]
    [InlineData("FA董adsf2313")]
    [InlineData("Password123§")]
    [InlineData("Password 123")]
    [InlineData("Password123  ")]
    [InlineData("  Password123")]
    public void validation_fails_for_passwords_with_not_allowed_characters(string newPassword)
    {
        // Arrange
        var command = new ResetPasswordCommand("email@gmail.com", "ABCDEF", newPassword, newPassword);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Token);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
        result.ShouldHaveValidationErrorFor(c => c.NewPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.ConfirmNewPassword);
    }
    
    [Fact]
    public void validation_fails_if_password_do_not_match_to_confirmation_password()
    {
        // Arrange
        const string newPassword = "Password123";
        const string confirmPassword = "123Password";
        var command = new ResetPasswordCommand("email@gmail.com", "ABCDEF", newPassword, confirmPassword);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Token);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
        result.ShouldHaveValidationErrorFor(c => c.NewPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.ConfirmNewPassword);
    }
}