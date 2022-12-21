using Application.Models.Filters;
using Application.Models.Pagination;
using Application.Users.Queries.GetAllUsers;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Users.Queries.GetAllUsers;

public class GetAllUsersQueryValidatorTest
{
    private readonly GetAllUserQueryValidator _validator;

    public GetAllUsersQueryValidatorTest()
    {
        _validator = new GetAllUserQueryValidator();
    }

    [Fact]
    public void Validation_passes_for_correct_data()
    {
        // Arrange
        var filter = new UserFilterDto(null, null, null);
        var pageRequest = new PageRequestDto { PageNumber = 1, PageSize = 10, Sort = null };
        
        var query = new GetAllUsersQuery(filter, pageRequest);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.PageRequestDto);
    }
}