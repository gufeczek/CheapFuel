using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.Models;
using Domain.Common.Pagination.Request;
using Domain.Common.Pagination.Response;
using FluentAssertions;
using WebAPI.IntegrationTests.PredefinedData;
using WebAPI.IntegrationTests.TestConfiguration;
using Xunit;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTests.FuelType;

public class FuelTypeQueryControllerTest : IntegrationTest
{
    public FuelTypeQueryControllerTest(
        TestingWebApiFactory<Program> factory, 
        ITestOutputHelper outputHelper)
        : base(
            factory, 
            outputHelper, 
            new IPredefinedData[] { new FuelTypeQueryControllerData() }) { }

    [Fact]
    public async Task Returns_page_of_fuel_types()
    {
        // Arrange
        const int pageNumber = 1;
        const int pageSize = 10;
        const string sortBy = nameof(FuelTypeDto.Id);
        const SortDirection direction = SortDirection.Asc;
        
        // Act 
        var response = await HttpClient.GetAsync(
            $"api/v1/fuel-types?" +
            $"PageNumber={pageNumber}&" +
            $"PageSize={pageSize}&" +
            $"Sort.SortBy={sortBy}&" +
            $"Sort.SortDirection={direction}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var page = await this.Deserialize<Page<FuelTypeDto>>(response.Content);
        page!.PageNumber.Should().Be(pageNumber);
        page.PageSize.Should().Be(pageSize);
        page.TotalElements.Should().Be(FuelTypeQueryControllerData.InitialFuelTypesCount);
        page.Data.Should().NotBeNull().And.HaveCount(FuelTypeQueryControllerData.InitialFuelTypesCount);
    }
}