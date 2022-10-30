using Application.Models.Pagination;
using Application.StationChains.Queries.GetAllStationChains;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.StationChains.Queries.GetAllStationChains;

public class GetAllStationChainsQueryValidatorTest
{
    private readonly GetAllStationChainsQueryValidator _validator;

    public GetAllStationChainsQueryValidatorTest()
    {
        _validator = new GetAllStationChainsQueryValidator();
    }

    [Fact]
    public void Validation_passes_for_correct_data()
    {
        // Arrange
        var pageRequest = new PageRequestDto { PageNumber = 1, PageSize = 10, Sort = null };
        var query = new GetAllStationChainsQuery(pageRequest);

        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.PageRequestDto);
    }
}