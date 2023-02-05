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

namespace WebAPI.IntegrationTests.Review;

public class ReviewQueryControllerTest : IntegrationTest
{
    public ReviewQueryControllerTest(
        TestingWebApiFactory<Program> factory, 
        ITestOutputHelper outputHelper) 
        : base(
            factory, 
            outputHelper, 
            new IPredefinedData[] { new AccountsData(), new ReviewQueryControllerData() }) { }

    [Fact]
    public async Task Returns_page_of_reviews()
    {
        // Arrange
        await this.AuthorizeUser();
        
        const int fuelStationId = ReviewQueryControllerData.FuelStation1Id;
        const int pageNumber = 1;
        const int pageSize = 10;
        const string sortBy = nameof(FuelStationReviewDto.CreatedAt);
        const SortDirection direction = SortDirection.Asc;
        
        // Act
        var response = await HttpClient.GetAsync(
            $"api/v1/reviews/fuel-station/{fuelStationId}?" +
            $"PageNumber={pageNumber}&" +
            $"PageSize={pageSize}&" +
            $"Sort.SortBy={sortBy}&" +
            $"Sort.SortDirection={direction}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var page = await this.Deserialize<Page<FuelStationReviewDto>>(response.Content);
        page!.PageNumber.Should().Be(pageNumber);
        page.PageSize.Should().Be(pageSize);
        page.TotalElements.Should().Be(ReviewQueryControllerData.FuelStation1InitialReviewCount);
        page.Data.Should().NotBeNull().And.HaveCount(ReviewQueryControllerData.FuelStation1InitialReviewCount);
    }

    [Fact]
    public async Task Returns_page_of_reviews_with_empty_list_of_reviews_for_fuel_station_without_reviews()
    {
        // Arrange
        await this.AuthorizeUser();

        const int fuelStationId = ReviewQueryControllerData.FuelStation2Id;
        const int pageNumber = 1;
        const int pageSize = 10;
        const string sortBy = nameof(FuelStationReviewDto.CreatedAt);
        const SortDirection direction = SortDirection.Asc;
        
        // Act
        var response = await HttpClient.GetAsync(
            $"api/v1/reviews/fuel-station/{fuelStationId}?" +
            $"PageNumber={pageNumber}&" +
            $"PageSize={pageSize}&" +
            $"Sort.SortBy={sortBy}&" +
            $"Sort.SortDirection={direction}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var page = await this.Deserialize<Page<FuelStationReviewDto>>(response.Content);
        page!.PageNumber.Should().Be(pageNumber);
        page.PageSize.Should().Be(pageSize);
        page.TotalElements.Should().Be(0);
        page.Data.Should().NotBeNull().And.BeEmpty();
    }
    
    [Fact]
    public async Task Fails_to_return_page_of_reviews_for_not_existing_fuel_station()
    {
        // Arrange
        await this.AuthorizeUser();

        const int fuelStationId = ReviewQueryControllerData.InvalidFuelStationId;
        const int pageNumber = 1;
        const int pageSize = 10;
        const string sortBy = nameof(FuelStationReviewDto.CreatedAt);
        const SortDirection direction = SortDirection.Asc;
        
        // Act
        var response = await HttpClient.GetAsync(
            $"api/v1/reviews/fuel-station/{fuelStationId}?" +
            $"PageNumber={pageNumber}&" +
            $"PageSize={pageSize}&" +
            $"Sort.SortBy={sortBy}&" +
            $"Sort.SortDirection={direction}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }

    [Fact]
    public async Task Returns_fuel_station_review()
    {
        // Arrange
        await this.AuthorizeUser();

        const string username = ReviewQueryControllerData.User1Username;
        const long fuelStationId = ReviewQueryControllerData.FuelStation1Id;
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/reviews/{fuelStationId}/{username}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var review = await this.Deserialize<FuelStationReviewDto>(response.Content);
        review.Should().NotBeNull();
        review!.Username.Should().Be(username);
        review.FuelStationId.Should().Be(fuelStationId);
    }
    
    [Fact]
    public async Task Fails_to_return_fuel_station_review_for_not_existing_user()
    {
        // Arrange
        await this.AuthorizeUser();

        const string username = AccountsData.InvalidUsername;
        const long fuelStationId = ReviewQueryControllerData.FuelStation1Id;
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/reviews/{fuelStationId}/{username}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }
    
    [Fact]
    public async Task Fails_to_return_fuel_station_review_for_not_existing_fuel_station()
    {
        // Arrange
        await this.AuthorizeUser();

        const string username = ReviewQueryControllerData.User1Username;
        const long fuelStationId = ReviewQueryControllerData.InvalidFuelStationId;
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/reviews/{fuelStationId}/{username}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }

    [Fact]
    public async Task Fails_to_return_fuel_station_review_if_given_user_not_reviewed_this_fuel_station()
    {
        // Arrange
        await this.AuthorizeUser();

        const string username = ReviewQueryControllerData.User3Username;
        const long fuelStationId = ReviewQueryControllerData.FuelStation1Id;
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/reviews/{fuelStationId}/{username}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }
}