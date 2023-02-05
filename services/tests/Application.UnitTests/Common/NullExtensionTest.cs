using System;
using Application.Common;
using FluentAssertions;
using Xunit;

namespace Application.UnitTests.Common;

public class NullExtensionTest
{
    [Fact]
    public void Returns_not_nullable_value_if_value_of_nullable_type_is_not_equal_to_null_and_is_managed()
    {
        // Arrange
        int? value = 1;
        
        // Act
        var result = value.OrElseThrow();
        
        // Assert
        result.Should().BeOfType(typeof(int));
        result.Should().Be(result);
    }
    
    [Fact]
    public void Returns_not_nullable_value_if_value_of_non_nullable_type_is_not_equal_to_null_and_is_managed()
    {
        // Arrange
        const int value = 1;
        
        // Act
        var result = value.OrElseThrow();
        
        // Assert
        result.Should().BeOfType(typeof(int));
        result.Should().Be(result);
    }

    [Fact]
    public void Throws_exception_if_value_of_nullable_type_is_equal_to_null()
    {
        // Arrange
        int? value = null;
        
        // Act
        Action act = () => value.OrElseThrow();
        
        // Assert
        act.Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void Throws_exception_if_value_of_non_nullable_type_is_equal_to_null()
    {
        // Arrange
        string value = null!;
        
        // Act
        Action act = () => value.OrElseThrow();
        
        // Assert
        act.Should()
            .Throw<ArgumentNullException>();
    }
}