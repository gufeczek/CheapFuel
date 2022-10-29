using Application.FuelStationServices.Queries.GetAllFuelStationServices;
using Application.Models.Pagination;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.FuelStationServices.Queries.GetAllFuelStationServices;

public class GetAllFuelStationServicesQueryValidatorTest
{
    private readonly GetAllFuelStationServicesQueryValidator _validator;

    public GetAllFuelStationServicesQueryValidatorTest()
    {
        _validator = new GetAllFuelStationServicesQueryValidator();
    }

    [Fact]
    public void Validation_passes_for_correct_data()
    {
        // Arrange
        var pageRequest = new PageRequestDto { PageNumber = 1, PageSize = 10, Sort = null };
        var query = new GetAllFuelStationServicesQuery(pageRequest);

        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.PageRequestDto);
    }
}