using Application.Users.Commands.ChangePassword;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Users.Commands.ChangePassword;

public class ChangeUserRoleCommandValidatorTest
{
    private readonly ChangePasswordCommandValidator _validator;

    public ChangeUserRoleCommandValidatorTest()
    {
        _validator = new ChangePasswordCommandValidator();
    }

    [Theory]
    [InlineData("Password", "Password123")]
    [InlineData("P", "@fhA123|123")]
    [InlineData("__", "Fab123Fgaa")]
    public void validation_passes_for_correct_data(string oldPassword, string newPassword)
    {
        // Arrange
        var command = new ChangePasswordCommand(oldPassword, newPassword, newPassword);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.OldPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.NewPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.ConfirmNewPassword);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void validation_fails_for_empty_old_password(string oldPassword)
    {
        // Arrange
        var command = new ChangePasswordCommand(oldPassword, "Password123", "Password123");
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.OldPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.NewPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.ConfirmNewPassword);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void validation_fails_for_empty_new_password(string newPassword)
    {
        // Arrange
        var command = new ChangePasswordCommand("Password123", newPassword, newPassword);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.OldPassword);
        result.ShouldHaveValidationErrorFor(c => c.NewPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.ConfirmNewPassword);
    }
    
    [Fact]
    public void validation_fails_for_new_passwords_with_more_than_32_characters()
    {
        // Arrange
        const string newPassword = "kbCEBglemEMw9Q3MuMmi42CQqRBOb7djf";
        var command = new ChangePasswordCommand("Password123", newPassword, newPassword);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.OldPassword);
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
        var command = new ChangePasswordCommand("Password123", newPassword, newPassword);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.OldPassword);
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
    public void validation_fails_for_new_passwords_with_not_allowed_characters(string newPassword)
    {
        // Arrange
        var command = new ChangePasswordCommand("Password123", newPassword, newPassword);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.OldPassword);
        result.ShouldHaveValidationErrorFor(c => c.NewPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.ConfirmNewPassword);
    }
    
    [Fact]
    public void validation_fails_if_new_password_do_not_match_to_confirmation_password()
    {
        // Arrange
        const string newPassword = "Password123";
        const string confirmPassword = "123Password";
        var command = new ChangePasswordCommand("Password123", newPassword, confirmPassword);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.OldPassword);
        result.ShouldHaveValidationErrorFor(c => c.NewPassword);
        result.ShouldNotHaveValidationErrorFor(c => c.ConfirmNewPassword);
    }
}