using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.Models;
using Application.StationChains.Commands.CreateStationChain;
using FluentAssertions;
using WebAPI.IntegrationTests.PredefinedData;
using WebAPI.IntegrationTests.TestConfiguration;
using Xunit;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTests.StationChain;

public class StationChainCommandControllerTest : IntegrationTest
{
    public StationChainCommandControllerTest(
        TestingWebApiFactory<Program> factory,
        ITestOutputHelper outputHelper)
        : base(
            factory,
            outputHelper,
            new IPredefinedData[] { new AccountsData(), new StationChainQueryControllerData() }) { }

    [Fact]
    public async Task Creates_new_station_chain()
    {
        // Arrange
        await this.AuthorizeAdmin();

        var command = new CreateStationChainCommand("Orlen");
        var body = this.Serialize(command);

        // Act
        var response = await HttpClient.PostAsync("api/v1/station-chains", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var createdObject = await this.Deserialize<StationChainDto>(response.Content);
        createdObject!.Id.Should().NotBe(null);
        createdObject!.Name.Should().Be("Orlen");

        this.CountAll<Domain.Entities.StationChain>().Should()
            .Be(StationChainCommandControllerData.InitialStationChainsCount + 1);
    }

    [Fact]
    public async Task Fails_to_create_new_station_if_not_admin()
    {
        // Arrange
        await this.AuthorizeUser();
        
        var f = this.AuthorizeAdmin;

        var command = new CreateStationChainCommand("Orlen");
        var body = this.Serialize(command);

        // Act
        var response = await HttpClient.PostAsync("api/v1/station-chains", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Domain.Entities.StationChain>().Should()
            .Be(StationChainCommandControllerData.InitialStationChainsCount);
    }

    [Fact]
    public async Task Fails_to_create_new_station_chain_when_name_is_to_short()
    {
        // Arrange
        await this.AuthorizeAdmin();

        var command = new CreateStationChainCommand("O");
        var body = this.Serialize(command);

        // Act
        var response = await HttpClient.PostAsync("api/v1/station-chains", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Domain.Entities.StationChain>().Should()
            .Be(StationChainCommandControllerData.InitialStationChainsCount);
    }

    [Fact]
    public async Task Deletes_station_chain_with_given_id()
    {
        // Arrange
        await this.AuthorizeAdmin();

        const int id = StationChainCommandControllerData.StationChainId1;

        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/station-chains/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        this.CountAll<Domain.Entities.StationChain>().Should()
            .Be(StationChainCommandControllerData.InitialStationChainsCount - 1);
    }

    [Fact]
    public async Task Fails_to_delete_station_chain_if_not_admin()
    {
        // Arrange
        await this.AuthorizeUser();

        const int id = StationChainCommandControllerData.InvalidId;

        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/station-chains/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        this.CountAll<Domain.Entities.StationChain>().Should()
            .Be(StationChainCommandControllerData.InitialStationChainsCount);
    }

    [Fact]
    public async Task Fails_to_delete_station_chain_for_wrong_id()
    {
        // Arrange
        await this.AuthorizeAdmin();

        const int id = StationChainCommandControllerData.InvalidId;

        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/station-chains/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        this.CountAll<Domain.Entities.StationChain>().Should()
            .Be(StationChainCommandControllerData.InitialStationChainsCount);
    }
}