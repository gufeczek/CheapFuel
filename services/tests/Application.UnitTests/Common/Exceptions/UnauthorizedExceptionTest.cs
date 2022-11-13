using Application.Common.Exceptions;
using FluentAssertions;
using Xunit;

namespace Application.UnitTests.Common.Exceptions;

public class UnauthorizedExceptionTest
{
    [Fact]
    public void should_create_exception_instance_with_message()
    {
        // Arrange
        const string message = "Test message";
        
        // Act
        var result = new UnauthorizedException(message);
        
        // Assert
        result.Message.Should().Be(message);
    }
}