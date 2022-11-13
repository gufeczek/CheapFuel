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

namespace WebAPI.IntegrationTests.FuelStationService;

public class FuelStationServiceQueryControllerTest : IntegrationTest
{
    public FuelStationServiceQueryControllerTest(
        TestingWebApiFactory<Program> factory, 
        ITestOutputHelper outputHelper)
        : base(
            factory, 
            outputHelper, 
            new IPredefinedData[] { new FuelStationServiceQueryControllerData() }) { }

    [Fact]
    public async Task Returns_page_of_fuel_station_services()
    {
        // Arrange
        const int pageNumber = 1;
        const int pageSize = 10;
        const string sortBy = nameof(FuelStationServiceDto.Id);
        const SortDirection direction = SortDirection.Asc;
        
        // Act 
        var response = await HttpClient.GetAsync(
            "api/v1/services?" +
            $"PageNumber={pageNumber}&" +
            $"PageSize={pageSize}&" +
            $"Sort.SortBy={sortBy}&" +
            $"Sort.SortDirection={direction}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var page = await this.Deserialize<Page<FuelStationServiceDto>>(response.Content);
        page!.PageNumber.Should().Be(pageNumber);
        page.PageSize.Should().Be(pageSize);
        page.TotalElements.Should().Be(FuelStationServiceQueryControllerData.InitialFuelStationServiceCount);
        page.Data.Should().NotBeNull().And.HaveCount(FuelStationServiceQueryControllerData.InitialFuelStationServiceCount);
    }
}