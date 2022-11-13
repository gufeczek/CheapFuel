using Application.Common.Exceptions;
using FluentAssertions;
using Xunit;

namespace Application.UnitTests.Common.Exceptions;

public class NotFoundExceptionTest
{
    [Fact]
    public void should_create_exception_instance_with_message()
    {
        // Arrange
        const string message = "Test message";
        
        // Act
        var result = new NotFoundException(message);
        
        // Assert
        result.Message.Should().Be(message);
    }

    [Fact]
    public void should_construct_message_from_given_values()
    {
        // Arrange
        const string entityName = "Entity";
        const string paramName = "Id";
        const string value = "1";
        
        // Act
        var result = new NotFoundException(entityName, paramName, value);
        
        // Assert
        result.Message.Should().Be($"{entityName} not found for {paramName} = {value}");
    }
}