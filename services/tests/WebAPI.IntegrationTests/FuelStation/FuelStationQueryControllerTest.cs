using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Filters;
using FluentAssertions;
using WebAPI.IntegrationTests.PredefinedData;
using WebAPI.IntegrationTests.TestConfiguration;
using Xunit;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTests.FuelStation;

public class FuelStationQueryControllerTest : IntegrationTest
{
    public FuelStationQueryControllerTest(
        TestingWebApiFactory<Program> factory,
        ITestOutputHelper outputHelper) 
        : base(
            factory, 
            outputHelper, 
            new IPredefinedData[] { new AccountsData(), new FuelStationQueryControllerData() }) { }

    [Fact]
    public async Task Returns_fuel_stations_with_price_of_a_given_fuel_type()
    {
        // Arrange
        await this.AuthorizeUser();
        
        const int fuelTypeId = FuelStationQueryControllerData.FuelType2Id;

        var filter = new FuelStationFilterDto(fuelTypeId, null, null, null, null);
        var body = this.Serialize(filter);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-stations/map", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var fuelStations = (await this.Deserialize<IEnumerable<SimpleFuelStationDto>>(response.Content))!.ToList();
        fuelStations.Count.Should().Be(FuelStationQueryControllerData.FuelStationsWithPriceOfFuelType1Count);
        fuelStations.Select(f => new { f.Id, f.Price }).Should().BeEquivalentTo(new[]
        {
            new { Id = FuelStationQueryControllerData.FuelStation2Id, Price = 4.56M },
            new { Id = FuelStationQueryControllerData.FuelStation3Id, Price = 2.52M }
        });
    }

    [Fact]
    public async Task Returns_fuel_stations_with_only_specified_service()
    {
        // Arrange
        await this.AuthorizeUser();

        const int fuelTypeId = FuelStationQueryControllerData.FuelType1Id;
        const int serviceId = FuelStationQueryControllerData.Service1Id;
        
        var filter = new FuelStationFilterDto(fuelTypeId, new List<long> { serviceId }, null, null, null);
        var body = this.Serialize(filter);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-stations/map", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var fuelStations = (await this.Deserialize<IEnumerable<SimpleFuelStationDto>>(response.Content))!.ToList();
        fuelStations.Count.Should().Be(2);
        fuelStations.Select(f => f.Id).Should().BeEquivalentTo(new[]
        {
            FuelStationQueryControllerData.FuelStation1Id,
            FuelStationQueryControllerData.FuelStation2Id
        });
    }
    
    [Fact]
    public async Task Returns_fuel_stations_for_multiple_specified_services()
    {
        // Arrange
        await this.AuthorizeUser();

        const int fuelTypeId = FuelStationQueryControllerData.FuelType1Id;
        const int service1Id = FuelStationQueryControllerData.Service1Id;
        const int service2Id = FuelStationQueryControllerData.Service2Id;

        var filter = new FuelStationFilterDto(fuelTypeId, new List<long> { service1Id, service2Id }, null, null, null);
        var body = this.Serialize(filter);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-stations/map", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var fuelStations = (await this.Deserialize<IEnumerable<SimpleFuelStationDto>>(response.Content))!.ToList();
        fuelStations.Count.Should().Be(3);
        fuelStations.Select(f => f.Id).Should().BeEquivalentTo(new[]
        {
            FuelStationQueryControllerData.FuelStation1Id,
            FuelStationQueryControllerData.FuelStation2Id,
            FuelStationQueryControllerData.FuelStation3Id
        });
    }

    [Fact]
    public async Task Returns_fuel_stations_from_only_specified_station_chain()
    {
        // Arrange
        await this.AuthorizeUser();

        const int fuelTypeId = FuelStationQueryControllerData.FuelType1Id;
        const int stationChainId = FuelStationQueryControllerData.StationChain1Id;
        
        var filter = new FuelStationFilterDto(fuelTypeId, null, new List<long>{ stationChainId }, null, null);
        var body = this.Serialize(filter);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-stations/map", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var fuelStations = (await this.Deserialize<IEnumerable<SimpleFuelStationDto>>(response.Content))!.ToList();
        fuelStations.Count.Should().Be(2);
        fuelStations.Select(f => f.Id).Should().BeEquivalentTo(new[]
        {
            FuelStationQueryControllerData.FuelStation1Id,
            FuelStationQueryControllerData.FuelStation2Id
        });
        fuelStations.All(f => f.StationChainName == FuelStationQueryControllerData.StationChain1Name).Should().BeTrue();
    }
    
    [Fact]
    public async Task Returns_fuel_stations_from_multiple_specified_station_chain()
    {
        // Arrange
        await this.AuthorizeUser();

        const int fuelTypeId = FuelStationQueryControllerData.FuelType1Id;
        const int stationChain1Id = FuelStationQueryControllerData.StationChain1Id;
        const int stationChain2Id = FuelStationQueryControllerData.StationChain2Id;
        
        var filter = new FuelStationFilterDto(fuelTypeId, null, new List<long>{ stationChain1Id, stationChain2Id }, null, null);
        var body = this.Serialize(filter);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-stations/map", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var fuelStations = (await this.Deserialize<IEnumerable<SimpleFuelStationDto>>(response.Content))!.ToList();
        fuelStations.Count.Should().Be(4);
        fuelStations.Select(f => f.Id).Should().BeEquivalentTo(new[]
        {
            FuelStationQueryControllerData.FuelStation1Id,
            FuelStationQueryControllerData.FuelStation2Id,
            FuelStationQueryControllerData.FuelStation3Id,
            FuelStationQueryControllerData.FuelStation4Id
        });
        fuelStations.All(f => 
            f.StationChainName == FuelStationQueryControllerData.StationChain1Name || 
            f.StationChainName == FuelStationQueryControllerData.StationChain2Name)
            .Should().BeTrue();
    }
    
    [Fact]
    public async Task Returns_fuel_stations_from_specified_station_chain_with_specified_services()
    {
        // Arrange
        await this.AuthorizeUser();

        const int fuelTypeId = FuelStationQueryControllerData.FuelType1Id;
        const int stationChainId = FuelStationQueryControllerData.StationChain2Id;
        const int serviceId = FuelStationQueryControllerData.Service2Id;

        var filter = new FuelStationFilterDto(fuelTypeId, new List<long> { serviceId }, new List<long> { stationChainId }, null, null);
        var body = this.Serialize(filter);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-stations/map", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var fuelStations = (await this.Deserialize<IEnumerable<SimpleFuelStationDto>>(response.Content))!.ToList();
        fuelStations.Count.Should().Be(1);
        fuelStations.Select(f => f.Id).Should().BeEquivalentTo(new[]
        {
            FuelStationQueryControllerData.FuelStation3Id
        });
    }

    [Fact]
    public async Task Returns_fuel_stations_with_price_below_specified_max_price()
    {
        // Arrange
        await this.AuthorizeUser();

        const int fuelTypeId = FuelStationQueryControllerData.FuelType2Id;

        var filter = new FuelStationFilterDto(fuelTypeId, null, null, null, 3.0M);
        var body = this.Serialize(filter);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-stations/map", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var fuelStations = (await this.Deserialize<IEnumerable<SimpleFuelStationDto>>(response.Content))!.ToList();
        fuelStations.Count.Should().Be(1);
        fuelStations.Select(f => new { f.Id, f.Price }).Should().BeEquivalentTo(new[]
        {
            new { Id = FuelStationQueryControllerData.FuelStation3Id, Price = 2.52M }
        });
    }
    
    [Fact]
    public async Task Returns_fuel_stations_with_price_above_specified_min_price()
    {
        // Arrange
        await this.AuthorizeUser();

        const int fuelTypeId = FuelStationQueryControllerData.FuelType2Id;

        var filter = new FuelStationFilterDto(fuelTypeId, null, null, 3.0M, null);
        var body = this.Serialize(filter);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-stations/map", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var fuelStations = (await this.Deserialize<IEnumerable<SimpleFuelStationDto>>(response.Content))!.ToList();
        fuelStations.Count.Should().Be(1);
        fuelStations.Select(f => new { f.Id, f.Price }).Should().BeEquivalentTo(new[]
        {
            new { Id = FuelStationQueryControllerData.FuelStation2Id, Price = 4.56M }
        });
    }
    
    [Fact]
    public async Task Returns_fuel_stations_with_price_in_given_price_range()
    {
        // Arrange
        await this.AuthorizeUser();

        const int fuelTypeId = FuelStationQueryControllerData.FuelType1Id;

        var filter = new FuelStationFilterDto(fuelTypeId, null, null, 2.0M, 3.0M);
        var body = this.Serialize(filter);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-stations/map", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var fuelStations = (await this.Deserialize<IEnumerable<SimpleFuelStationDto>>(response.Content))!.ToList();
        fuelStations.Count.Should().Be(2);
        fuelStations.Select(f => new { f.Id, f.Price }).Should().BeEquivalentTo(new[]
        {
            new { Id = FuelStationQueryControllerData.FuelStation1Id, Price = 2.14M },
            new { Id = FuelStationQueryControllerData.FuelStation3Id, Price = 2.51M }
        });
    }

    [Fact]
    public async Task Fails_to_return_fuel_stations_for_not_logged_user()
    {
        // Arrange
        const int fuelTypeId = FuelStationQueryControllerData.FuelType1Id;

        var filter = new FuelStationFilterDto(fuelTypeId, null, null, 2.0M, 3.0M);
        var body = this.Serialize(filter);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-stations/map", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }
    
    [Fact]
    public async Task Fails_to_returns_fuel_stations_for_not_existing_fuel_type()
    {
        // Arrange
        await this.AuthorizeUser();

        const int fuelTypeId = FuelStationQueryControllerData.InvalidFuelTypeId;

        var filter = new FuelStationFilterDto(fuelTypeId, null, null, 2.0M, 3.0M);
        var body = this.Serialize(filter);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-stations/map", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }
    
    [Fact]
    public async Task Fails_to_returns_fuel_stations_for_not_existing_service()
    {
        // Arrange
        await this.AuthorizeUser();

        const int fuelTypeId = FuelStationQueryControllerData.FuelType1Id;
        const int service1Id = FuelStationQueryControllerData.Service1Id;
        const int service2Id = FuelStationQueryControllerData.InvalidServiceId;
        
        var filter = new FuelStationFilterDto(fuelTypeId, new List<long> { service1Id, service2Id }, null, 2.0M, 3.0M);
        var body = this.Serialize(filter);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-stations/map", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }
    
    [Fact]
    public async Task Fails_to_returns_fuel_stations_for_not_existing_station_chain()
    {
        // Arrange
        await this.AuthorizeUser();

        const int fuelTypeId = FuelStationQueryControllerData.FuelType1Id;
        const int stationChain1Id = FuelStationQueryControllerData.StationChain1Id;
        const int stationChain2Id = FuelStationQueryControllerData.InvalidStationChainId;
        
        var filter = new FuelStationFilterDto(fuelTypeId, null, new List<long>{ stationChain1Id, stationChain2Id }, 2.0M, 3.0M);
        var body = this.Serialize(filter);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-stations/map", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }

    [Fact]
    public async Task Returns_fuel_stations_with_all_details()
    {
        // Arrange
        await this.AuthorizeUser();

        const int fuelStationId = FuelStationQueryControllerData.FuelStation1Id;
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/fuel-stations/{fuelStationId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var fuelStations = await this.Deserialize<FuelStationDetailsDto>(response.Content);
        
        fuelStations!.Id.Should().Be(fuelStationId);
        fuelStations.Name.Should().Be("Fuel station 1");
        fuelStations.StationChain!.Id.Should().Be(FuelStationQueryControllerData.StationChain1Id);
        fuelStations.OpeningClosingTimes.Should().NotBeNull();
        fuelStations.Address.Should().NotBeNull();
        fuelStations.Services!.Count().Should().Be(2);
        fuelStations.FuelTypes!.Count().Should().Be(1);
        fuelStations.FuelTypes!.ToList()[0].Should().NotBeNull();
        fuelStations.FuelTypes!.ToList()[0].FuelPrice!.Price.Should().Be(2.14M);
    }

    [Fact]
    public async Task Returns_fuel_station_details_with_all_fuel_types()
    {
        // Arrange
        await this.AuthorizeUser();

        const int fuelStationId = FuelStationQueryControllerData.FuelStation4Id;
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/fuel-stations/{fuelStationId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        
        var fuelStations = await this.Deserialize<FuelStationDetailsDto>(response.Content);
        
        fuelStations!.Id.Should().Be(fuelStationId);
        fuelStations.Name.Should().Be("Fuel station 4");
        fuelStations.StationChain!.Id.Should().Be(FuelStationQueryControllerData.StationChain2Id);
        fuelStations.OpeningClosingTimes.Should().NotBeNull();
        fuelStations.Address.Should().NotBeNull();
        fuelStations.Location.Should().NotBeNull();
        fuelStations.Services!.Count().Should().Be(0);
        fuelStations.FuelTypes!.Count().Should().Be(2);
        fuelStations.FuelTypes!
            .Select(f => f.FuelPrice)
            .Count(p => p is not null)
            .Should().Be(1);
    }

    [Fact]
    public async Task Fails_to_return_fuel_station_details_for_not_logged_user()
    {
        // Arrange
        const int fuelStationId = FuelStationQueryControllerData.FuelStation1Id;
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/fuel-stations/{fuelStationId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }
    
    [Fact]
    public async Task Fails_to_return_fuel_station_details_for_invalid_id()
    {
        // Arrange
        const int fuelStationId = FuelStationQueryControllerData.InvalidFuelStationId;
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/fuel-stations/{fuelStationId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }
}