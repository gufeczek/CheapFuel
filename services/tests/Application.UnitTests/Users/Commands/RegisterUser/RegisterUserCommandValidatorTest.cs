using Application.Users.Commands.RegisterUser;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Users.Commands.RegisterUser;

public class RegisterUserCommandValidatorTest
{
    private readonly RegisterUserCommandValidator _validator;

    public RegisterUserCommandValidatorTest()
    {
        _validator = new RegisterUserCommandValidator();
    }
    
    [Theory]
    [InlineData("Username", "username@email.com", "Password1")]
    [InlineData("123abc", "123abc@gmail.com", "@fhA123|123")]
    [InlineData("_23", "abc.def@mail-archive.com", "Fab123Fgaa")]
    public void validation_passes_for_correct_data(string username, string email, string password)
    {
        // Arrange
        var command = new RegisterUserCommand(username, email, password, password);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Username);
        result.ShouldNotHaveValidationErrorFor(c => c.Password);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void validation_fails_for_empty_username(string username)
    {
        // Arrange
        var command = new RegisterUserCommand(username, "email@gmail.com", "Password123", "Password123");
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Username);
        result.ShouldNotHaveValidationErrorFor(c => c.Password);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData(".b")]
    public void validation_fails_for_usernames_shorter_then_three_characters(string username)
    {
        // Arrange
        var command = new RegisterUserCommand(username, "email@gmail.com", "Password123", "Password123");
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Username);
        result.ShouldNotHaveValidationErrorFor(c => c.Password);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
    }
    
    [Fact]
    public void validation_fails_for_username_with_more_than_32_characters()
    {
        // Arrange
        const string username = "kbCEBglemEMw9Q3MuMmi42CQqRBOb7djf";
        var command = new RegisterUserCommand(username, "email@gmail.com", "Password123", "Password123");

        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Username);
        result.ShouldNotHaveValidationErrorFor(c => c.Password);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
    }

    [Theory]
    [InlineData("__a")]
    [InlineData("-2-")]
    [InlineData("a..")]
    [InlineData("__a__")]
    public void validation_fails_for_usernames_with_less_than_two_letters_or_digits(string username)
    {
        // Arrange
        var command = new RegisterUserCommand(username, "email@gmail.com", "Password123", "Password123");
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Username);
        result.ShouldNotHaveValidationErrorFor(c => c.Password);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
    }

    [Theory]
    [InlineData("username@")]
    [InlineData("Bożydar")]
    [InlineData("董文谭贺")]
    [InlineData("User Name")]
    [InlineData("   Username")]
    [InlineData("Username   ")]
    public void validation_fails_for_usernames_with_not_allowed_characters(string username)
    {
        // Arrange
        var command = new RegisterUserCommand(username, "email@gmail.com", "Password123", "Password123");
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Username);
        result.ShouldNotHaveValidationErrorFor(c => c.Password);
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
        var command = new RegisterUserCommand("Username", email, "Password123", "Password123");
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Username);
        result.ShouldNotHaveValidationErrorFor(c => c.Password);
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void validation_fails_for_empty_passwords(string password)
    {
        // Arrange
        var command = new RegisterUserCommand("Username", "email@gmail.com", password, password);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Username);
        result.ShouldHaveValidationErrorFor(c => c.Password);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("aB1")]
    [InlineData("abcDEF1")]
    public void validation_fails_for_passwords_shorter_then_eight_characters(string password)
    {
        // Arrange
        var command = new RegisterUserCommand("Username", "email@gmail.com", password, password);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Username);
        result.ShouldHaveValidationErrorFor(c => c.Password);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void validation_fails_for_passwords_with_more_than_32_characters()
    {
        // Arrange
        const string password = "kbCEBglemEMw9Q3MuMmi42CQqRBOb7djf";
        var command = new RegisterUserCommand("Username", "email@gmail.com", password, password);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Username);
        result.ShouldHaveValidationErrorFor(c => c.Password);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
    }
    
    [Theory]
    [InlineData("abcdefghti")]
    [InlineData("ABCDEFGHIFDS")]
    [InlineData("123456789")]
    [InlineData("abcdeABCD")]
    [InlineData("1234advfds")]
    [InlineData("AFFDS1234")]
    public void validation_fails_for_passwords_without_at_least_one_lowercase_and_uppercase_letter_and_one_digit(string password)
    {
        // Arrange
        var command = new RegisterUserCommand("Username", "email@gmail.com", password, password);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Username);
        result.ShouldHaveValidationErrorFor(c => c.Password);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
    }
    
    [Theory]
    [InlineData("Bożydar1234")]
    [InlineData("FA董adsf2313")]
    [InlineData("Password123§")]
    [InlineData("Password 123")]
    [InlineData("Password123  ")]
    [InlineData("  Password123")]
    public void validation_fails_for_passwords_with_not_allowed_characters(string password)
    {
        // Arrange
        var command = new RegisterUserCommand("Username", "email@gmail.com", password, password);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Username);
        result.ShouldHaveValidationErrorFor(c => c.Password);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void validation_fails_if_password_do_not_match_to_confirmation_password()
    {
        // Arrange
        const string password = "Password123";
        const string confirmPassword = "123Password";
        var command = new RegisterUserCommand("Username", "email@gmail.com", password, confirmPassword);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Username);
        result.ShouldHaveValidationErrorFor(c => c.Password);
        result.ShouldNotHaveValidationErrorFor(c => c.Email);
    }
}