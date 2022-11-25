using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.Models;
using FluentAssertions;
using WebAPI.IntegrationTests.PredefinedData;
using WebAPI.IntegrationTests.TestConfiguration;
using Xunit;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTests.Favourite;

public class FavouriteQueryControllerTest : IntegrationTest
{
    public FavouriteQueryControllerTest(
        TestingWebApiFactory<Program> factory, 
        ITestOutputHelper outputHelper) 
        : base(
            factory, 
            outputHelper,
            new IPredefinedData[] { new AccountsData(), new FavouriteQueryControllerData() }) { }

    [Fact]
    public async Task Returns_favourite_fuel_station_of_user()
    {
        // Arrange
        await this.AuthorizeUser();

        const string username = FavouriteQueryControllerData.UserWithFavouriteUsername;
        const int fuelStationId = FavouriteQueryControllerData.FuelStation1Id;
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/favourites/{username}/{fuelStationId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var favourite = await this.Deserialize<SimpleUserFavouriteDto>(response.Content);
        favourite!.Username.Should().Be(username);
        favourite.FuelStationId.Should().Be(fuelStationId);
        favourite.CreatedAt.Should().NotBe(null);
    }

    [Fact]
    public async Task Fails_to_return_favourite_if_user_is_not_logged_in()
    {
        // Arrange
        const string username = FavouriteQueryControllerData.UserWithFavouriteUsername;
        const int fuelStationId = FavouriteQueryControllerData.FuelStation1Id;
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/favourites/{username}/{fuelStationId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }

    [Fact]
    public async Task Fails_to_return_favourite_if_user_not_exists()
    {
        // Arrange
        await this.AuthorizeUser();

        const string username = AccountsData.InvalidUsername;
        const int fuelStationId = FavouriteQueryControllerData.InvalidFuelStationId;
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/favourites/{username}/{fuelStationId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }
    
    [Fact]
    public async Task Fails_to_return_favourite_if_fuel_station_not_exists()
    {
        // Arrange
        await this.AuthorizeUser();

        const string username = FavouriteQueryControllerData.UserWithFavouriteUsername;
        const int fuelStationId = FavouriteQueryControllerData.InvalidFuelStationId;
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/favourites/{username}/{fuelStationId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }

    [Fact]
    public async Task Fails_to_return_favourite_if_user_does_not_have_fuel_station_in_his_favourites()
    {
        // Arrange
        await this.AuthorizeUser();

        const string username = FavouriteQueryControllerData.UserWithoutFavouriteUsername;
        const int fuelStationId = FavouriteQueryControllerData.FuelStation1Id;
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/favourites/{username}/{fuelStationId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }
}