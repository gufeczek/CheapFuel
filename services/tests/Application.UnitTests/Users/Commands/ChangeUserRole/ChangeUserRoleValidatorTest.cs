using Application.Users.Commands.ChangeUserRole;
using Domain.Enums;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Users.Commands.ChangeUserRole;

public class ChangeUserRoleValidatorTest
{
    private readonly ChangeUserRoleCommandValidator _validator;

    public ChangeUserRoleValidatorTest()
    {
        _validator = new ChangeUserRoleCommandValidator();
    }

    [Theory]
    [InlineData("Username", Role.User)]
    [InlineData("123", Role.Owner)]
    [InlineData("%$@fsd", Role.Admin)]
    public void validation_passes_for_correct_data(string username, Role role)
    {
        // Arrange
        var command = new ChangeUserRoleCommand(username, role);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Username);
        result.ShouldNotHaveValidationErrorFor(c => c.Role);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void validation_fails_for_empty_username(string username)
    {
        // Arrange
        var command = new ChangeUserRoleCommand(username, Role.User);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Username);
        result.ShouldNotHaveValidationErrorFor(c => c.Role);
    }

    [Fact]
    public void validation_fails_for_empty_role()
    {
        // Arrange
        var command = new ChangeUserRoleCommand("Username", null);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Username);
        result.ShouldHaveValidationErrorFor(c => c.Role);

    }
}