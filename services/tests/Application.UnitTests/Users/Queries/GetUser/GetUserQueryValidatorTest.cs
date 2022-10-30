using Application.Users.Queries.GetUser;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Users.Queries.GetUser;

public class GetUserQueryValidatorTest
{
    private readonly GetUserQueryValidator _validator;

    public GetUserQueryValidatorTest()
    {
        _validator = new GetUserQueryValidator();
    }
    
    [Theory]
    [InlineData("Username")]
    [InlineData("1")]
    [InlineData("_f3#$$")]
    public void validation_passes_for_correct_data(string username)
    {
        // Arrange
        var query = new GetUserQuery(username);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Username);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void validation_fails_for_empty_username(string username)
    {
        // Arrange
        var query = new GetUserQuery(username);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Username);
    }
}