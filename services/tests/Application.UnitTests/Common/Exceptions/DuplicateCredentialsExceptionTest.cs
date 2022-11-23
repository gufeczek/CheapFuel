using Application.Common.Exceptions;
using FluentAssertions;
using Xunit;

namespace Application.UnitTests.Common.Exceptions;

public class DuplicateCredentialsExceptionTest
{
    [Fact]
    public void should_create_exception_instance_with_message()
    {
        // Arrange
        const string message = "Test message";
        
        // Act
        var result = new DuplicateCredentialsException(message);
        
        // Assert
        result.Message.Should().Be(message);
    }
}