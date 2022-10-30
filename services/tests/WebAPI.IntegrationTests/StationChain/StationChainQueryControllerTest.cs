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

namespace WebAPI.IntegrationTests.StationChain;

public class StationChainQueryControllerTest : IntegrationTest
{
    public StationChainQueryControllerTest(
        TestingWebApiFactory<Program> factory, 
        ITestOutputHelper outputHelper)
        : base(
            factory, 
            outputHelper, 
            new IPredefinedData[] { new StationChainQueryControllerData() }) { }
    
    [Fact]
    public async Task Returns_page_of_station_chains()
    {
        // Arrange
        const int pageNumber = 1;
        const int pageSize = 10;
        const string sortBy = nameof(StationChainDto.Id);
        const SortDirection direction = SortDirection.Asc;
        
        // Act 
        var response = await HttpClient.GetAsync(
            $"api/v1/station-chains?" +
            $"PageNumber={pageNumber}&" +
            $"PageSize={pageSize}&" +
            $"Sort.SortBy={sortBy}&" +
            $"Sort.SortDirection={direction}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var page = await this.Deserialize<Page<StationChainDto>>(response.Content);
        page!.PageNumber.Should().Be(pageNumber);
        page.PageSize.Should().Be(pageSize);
        page.TotalElements.Should().Be(StationChainQueryControllerData.InitialStationChainsCount);
        page.Data.Should().NotBeNull().And.HaveCount(StationChainQueryControllerData.InitialStationChainsCount);
    }
}