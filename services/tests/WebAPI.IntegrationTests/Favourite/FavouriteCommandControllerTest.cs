using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.Favorites.Commands.CreateFavourite;
using Application.Models;
using Domain.Entities;
using FluentAssertions;
using WebAPI.IntegrationTests.PredefinedData;
using WebAPI.IntegrationTests.TestConfiguration;
using Xunit;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTests.Favourite;

public class FavouriteCommandControllerTest : IntegrationTest
{
    public FavouriteCommandControllerTest(
        TestingWebApiFactory<Program> factory, 
        ITestOutputHelper outputHelper) 
        : base(
            factory, 
            outputHelper,
            new IPredefinedData[] { new AccountsData(), new FavouriteCommandControllerData() }) { }

    [Fact]
    public async Task Adds_fuel_station_to_favourite_of_logged_user()
    {
        // Arrange
        await this.AuthorizeGenericUser(FavouriteCommandControllerData.UserWithoutFavouriteUsername, AccountsData.DefaultPassword);

        const int fuelStationId = FavouriteCommandControllerData.FuelStation1Id;

        var command = new CreateFavouriteCommand(fuelStationId);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/favourites", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var favourite = await this.Deserialize<SimpleUserFavouriteDto>(response.Content);
        favourite!.Username.Should().Be(FavouriteCommandControllerData.UserWithoutFavouriteUsername);
        favourite.FuelStationId.Should().Be(fuelStationId);
        favourite.CreatedAt.Should().NotBe(null);

        var favourites = this.GetAll<Favorite>();
        favourites.Count().Should().Be(FavouriteCommandControllerData.InitialFavouriteCount + 1);
        favourites
            .Count(f => f.UserId == FavouriteCommandControllerData.UserWithoutFavouriteId)
            .Should()
            .Be(1);
    }
    
    [Fact]
    public async Task Fails_to_add_fuel_station_to_favourite_if_user_is_not_logged_in()
    {
        // Arrange
        const int fuelStationId = FavouriteCommandControllerData.FuelStation1Id;

        var command = new CreateFavouriteCommand(fuelStationId);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/favourites", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Favorite>()
            .Should()
            .Be(FavouriteCommandControllerData.InitialFavouriteCount);
    }

    [Fact]
    public async Task Fails_to_add_fuel_station_to_favourite_if_fuel_station_not_exists()
    {
        // Arrange
        await this.AuthorizeGenericUser(FavouriteCommandControllerData.UserWithoutFavouriteUsername, AccountsData.DefaultPassword);

        const int fuelStationId = FavouriteCommandControllerData.InvalidFuelStationId;

        var command = new CreateFavouriteCommand(fuelStationId);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/favourites", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Favorite>()
            .Should()
            .Be(FavouriteCommandControllerData.InitialFavouriteCount);
    }
    
    [Fact]
    public async Task Fails_to_add_fuel_station_to_favourite_if_user_already_has_this_fuel_station_in_his_favourites()
    {
        // Arrange
        await this.AuthorizeGenericUser(FavouriteCommandControllerData.UserWithFavouriteUsername, AccountsData.DefaultPassword);

        const int fuelStationId = FavouriteCommandControllerData.FuelStation1Id;

        var command = new CreateFavouriteCommand(fuelStationId);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/favourites", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Favorite>()
            .Should()
            .Be(FavouriteCommandControllerData.InitialFavouriteCount);
    }

    [Fact]
    public async Task Deletes_fuels_station_from_user_favourites()
    {
        // Arrange
        await this.AuthorizeGenericUser(FavouriteCommandControllerData.UserWithFavouriteUsername, AccountsData.DefaultPassword);

        const int fuelStationId = FavouriteCommandControllerData.FuelStation1Id;

        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/favourites/{fuelStationId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var favourites = this.GetAll<Favorite>();
        favourites.Count().Should().Be(FavouriteCommandControllerData.InitialFavouriteCount - 1);
        favourites
            .Count(f => f.UserId == FavouriteCommandControllerData.UserWithFavouriteId && f.FuelStationId == fuelStationId)
            .Should()
            .Be(0);
    }

    [Fact]
    public async Task Fails_to_delete_fuel_station_if_user_not_logged_in()
    {
        // Arrange
        const int fuelStationId = FavouriteCommandControllerData.FuelStation1Id;

        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/favourites/{fuelStationId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Favorite>()
            .Should()
            .Be(FavouriteCommandControllerData.InitialFavouriteCount);
    }
    
    [Fact]
    public async Task Fails_to_delete_fuel_station_if_user_does_not_have_given_fuel_station_in_his_favourites()
    {
        // Arrange
        await this.AuthorizeGenericUser(FavouriteCommandControllerData.UserWithoutFavouriteUsername, AccountsData.DefaultPassword);

        const int fuelStationId = FavouriteCommandControllerData.FuelStation1Id;

        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/favourites/{fuelStationId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Favorite>()
            .Should()
            .Be(FavouriteCommandControllerData.InitialFavouriteCount);
    }

    [Fact]
    public async Task Fails_to_delete_fuel_station_if_fuel_station_not_exists()
    {
        // Arrange
        await this.AuthorizeGenericUser(FavouriteCommandControllerData.UserWithFavouriteUsername, AccountsData.DefaultPassword);

        const int fuelStationId = FavouriteCommandControllerData.InvalidFuelStationId;

        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/favourites/{fuelStationId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Favorite>()
            .Should()
            .Be(FavouriteCommandControllerData.InitialFavouriteCount);
    }
}