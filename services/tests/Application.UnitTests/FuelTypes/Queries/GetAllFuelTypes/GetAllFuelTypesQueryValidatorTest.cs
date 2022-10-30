using Application.FuelTypes.Queries.GetAllFuelTypes;
using Application.Models.Pagination;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.FuelTypes.Queries.GetAllFuelTypes;

public class GetAllFuelTypesQueryValidatorTest
{
    private readonly GetAllFuelTypesQueryValidator _validator;

    public GetAllFuelTypesQueryValidatorTest()
    {
        _validator = new GetAllFuelTypesQueryValidator();
    }

    [Fact]
    public void Validation_passes_for_correct_data()
    {
        // Arrange
        var pageRequest = new PageRequestDto { PageNumber = 1, PageSize = 10, Sort = null };
        var query = new GetAllFuelTypesQuery(pageRequest);

        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.PageRequestDto);
    }
}