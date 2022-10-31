using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.FuelTypes.Commands.CreateFuelType;
using Application.Models;
using FluentAssertions;
using WebAPI.IntegrationTests.PredefinedData;
using WebAPI.IntegrationTests.TestConfiguration;
using Xunit;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTests.FuelType;

public class FuelTypeCommandControllerTest : IntegrationTest
{
    public FuelTypeCommandControllerTest(
        TestingWebApiFactory<Program> factory, 
        ITestOutputHelper outputHelper)
        : base(
            factory, 
            outputHelper, 
            new IPredefinedData[] { new AccountsData(), new FuelTypeCommandControllerData() }) { }

    [Fact]
    public async Task Creates_new_fuel_type()
    {
        // Arrange
        await this.AuthorizeAdmin();

        var command = new CreateFuelTypeCommand("98");
        var body = this.Serialize(command);

        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-types", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var createdObject = await this.Deserialize<FuelTypeDto>(response.Content);
        createdObject!.Id.Should().NotBe(null);
        createdObject!.Name.Should().Be("98");

        this.CountAll<Domain.Entities.FuelType>().Should()
            .Be(FuelTypeCommandControllerData.InitialFuelTypesCount + 1);
    }
    
    [Fact]
    public async Task Fails_to_create_fuel_type_if_not_admin()
    {
        // Arrange
        await this.AuthorizeUser();

        var command = new CreateFuelTypeCommand("98");
        var body = this.Serialize(command);

        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-types", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Domain.Entities.FuelType>().Should()
            .Be(FuelTypeCommandControllerData.InitialFuelTypesCount);
    }
    
    [Fact]
    public async Task Fails_to_create_fuel_type_chain_when_name_is_to_short()
    {
        // Arrange
        await this.AuthorizeAdmin();

        var command = new CreateFuelTypeCommand("");
        var body = this.Serialize(command);

        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-types", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Domain.Entities.FuelType>().Should()
            .Be(FuelTypeCommandControllerData.InitialFuelTypesCount);
    }
    
    [Fact]
    public async Task Deletes_fuel_type_with_given_id()
    {
        // Arrange
        await this.AuthorizeAdmin();

        const int id = FuelTypeCommandControllerData.FuelTypeId1;

        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/fuel-types/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        this.CountAll<Domain.Entities.FuelType>().Should()
            .Be(StationChainCommandControllerData.InitialStationChainsCount - 1);
    }
    
    [Fact]
    public async Task Fails_to_delete_fuel_type_if_not_admin()
    {
        // Arrange
        await this.AuthorizeAdmin();

        const int id = FuelTypeCommandControllerData.InvalidId;

        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/fuel-types/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        this.CountAll<Domain.Entities.FuelType>().Should()
            .Be(StationChainCommandControllerData.InitialStationChainsCount);
    }
}