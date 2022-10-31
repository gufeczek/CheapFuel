using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.FuelStationServices.Commands.CreateFuelStationService;
using Application.Models;
using FluentAssertions;
using WebAPI.IntegrationTests.PredefinedData;
using WebAPI.IntegrationTests.TestConfiguration;
using Xunit;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTests.FuelStationService;

public class FuelStationServiceCommandControllerTest : IntegrationTest
{
    public FuelStationServiceCommandControllerTest(
        TestingWebApiFactory<Program> factory,
        ITestOutputHelper outputHelper)
        : base(
            factory,
            outputHelper,
            new IPredefinedData[] { new AccountsData(), new FuelStationServiceCommandControllerData() }) { }

    [Fact]
    public async Task Creates_new_service()
    {
        // Arrange
        await this.AuthorizeAdmin();

        var command = new CreateFuelStationServiceCommand("Wash");
        var body = this.Serialize(command);

        // Act
        var response = await HttpClient.PostAsync("api/v1/services", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var createdObject = await this.Deserialize<FuelStationServiceDto>(response.Content);
        createdObject!.Id.Should().NotBe(null);
        createdObject.Name.Should().Be("Wash");

        this.CountAll<Domain.Entities.FuelStationService>().Should()
            .Be(FuelStationServiceCommandControllerData.InitialFuelStationServiceCount + 1);
    }

    [Fact]
    public async Task Fails_to_create_new_service_if_not_admin()
    {
        // Arrange
        await this.AuthorizeUser();
        
        var f = this.AuthorizeAdmin;

        var command = new CreateFuelStationServiceCommand("Wash");
        var body = this.Serialize(command);

        // Act
        var response = await HttpClient.PostAsync("api/v1/services", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Domain.Entities.FuelStationService>().Should()
            .Be(FuelStationServiceCommandControllerData.InitialFuelStationServiceCount);
    }

    [Fact]
    public async Task Fails_to_create_service_when_name_is_to_short()
    {
        // Arrange
        await this.AuthorizeAdmin();

        var command = new CreateFuelStationServiceCommand("O");
        var body = this.Serialize(command);

        // Act
        var response = await HttpClient.PostAsync("api/v1/services", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Domain.Entities.FuelStationService>().Should()
            .Be(FuelStationServiceCommandControllerData.InitialFuelStationServiceCount);
    }

    [Fact]
    public async Task Deletes_service_with_given_id()
    {
        // Arrange
        await this.AuthorizeAdmin();

        const int id = FuelStationServiceCommandControllerData.ServiceId1;

        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/services/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        this.CountAll<Domain.Entities.FuelStationService>().Should()
            .Be(FuelStationServiceCommandControllerData.InitialFuelStationServiceCount - 1);
    }

    [Fact]
    public async Task Fails_to_delete_service_if_not_admin()
    {
        // Arrange
        await this.AuthorizeUser();

        const int id = FuelStationServiceCommandControllerData.InvalidId;

        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/services/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        this.CountAll<Domain.Entities.FuelStationService>().Should()
            .Be(FuelStationServiceCommandControllerData.InitialFuelStationServiceCount);
    }

    [Fact]
    public async Task Fails_to_delete_service_for_wrong_id()
    {
        // Arrange
        await this.AuthorizeAdmin();

        const int id = FuelStationServiceCommandControllerData.InvalidId;

        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/services/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        this.CountAll<Domain.Entities.FuelStationService>().Should()
            .Be(FuelStationServiceCommandControllerData.InitialFuelStationServiceCount);
    }
}