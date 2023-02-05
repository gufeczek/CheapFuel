using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.Models.FuelPriceDtos;
using FluentAssertions;
using WebAPI.IntegrationTests.PredefinedData;
using WebAPI.IntegrationTests.TestConfiguration;
using Xunit;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTests.FuelPrice;

public class FuelPriceCommandControllerTest : IntegrationTest
{
    public FuelPriceCommandControllerTest(
        TestingWebApiFactory<Program> factory, 
        ITestOutputHelper outputHelper) 
        : base(
            factory, 
            outputHelper, 
            new IPredefinedData[] { new AccountsData(), new FuelPriceCommandControllerData() }) { }

    [Fact]
    public async Task Creates_new_fuel_prices_performed_by_owner()
    {
        // Arrange
        await this.AuthorizeGenericUser(FuelPriceCommandControllerData.FuelStationOwnerUsername, AccountsData.DefaultPassword);
        
        var dto = CreateDto();
        var body = this.Serialize(dto);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-prices/owner", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var createdObjects = await this.Deserialize<IEnumerable<FuelPriceDto>>(response.Content);
        createdObjects!.Count().Should().Be(2);

        this.CountAll<Domain.Entities.FuelPrice>().Should()
            .Be(FuelPriceCommandControllerData.FuelPriceInitialCount + 2);
    }
    
    [Fact]
    public async Task Creates_new_fuel_prices_performed_by_admin()
    {
        // Arrange
        await this.AuthorizeAdmin();
        
        var dto = CreateDto();
        var body = this.Serialize(dto);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-prices/owner", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var createdObjects = await this.Deserialize<IEnumerable<FuelPriceDto>>(response.Content);
        createdObjects!.Count().Should().Be(2);

        this.CountAll<Domain.Entities.FuelPrice>().Should()
            .Be(FuelPriceCommandControllerData.FuelPriceInitialCount + 2);
    }

    [Fact]
    public async Task Fails_to_create_new_fuel_prices_if_user_not_logged_in()
    {
        // Arrange
        var dto = CreateDto();
        var body = this.Serialize(dto);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-prices/owner", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Domain.Entities.FuelPrice>().Should()
            .Be(FuelPriceCommandControllerData.FuelPriceInitialCount);
    }

    [Fact]
    public async Task Fails_to_create_new_fuel_prices_if_not_owner()
    {
        // Arrange
        await this.AuthorizeUser();
        
        var dto = CreateDto();
        var body = this.Serialize(dto);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-prices/owner", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Domain.Entities.FuelPrice>().Should()
            .Be(FuelPriceCommandControllerData.FuelPriceInitialCount);
    }

    [Fact]
    public async Task Fails_to_create_new_fuel_prices_if_fuel_station_not_found()
    {
        // Arrange
        await this.AuthorizeGenericUser(FuelPriceCommandControllerData.FuelStationOwnerUsername, AccountsData.DefaultPassword);
        
        var dto = CreateDto();
        dto.FuelStationId = FuelPriceCommandControllerData.InvalidFuelStationId;
        
        var body = this.Serialize(dto);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-prices/owner", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Domain.Entities.FuelPrice>().Should()
            .Be(FuelPriceCommandControllerData.FuelPriceInitialCount);
    }
    
    [Fact]
    public async Task Fails_to_create_new_fuel_prices_if_user_send_fuel_prices_for_duplicate_fuel_types()
    {
        // Arrange
        await this.AuthorizeGenericUser(FuelPriceCommandControllerData.FuelStationOwnerUsername, AccountsData.DefaultPassword);
        
        var dto = CreateDto();
        dto.FuelPrices!.ForEach(f => f.FuelTypeId = FuelPriceCommandControllerData.FuelType1Id);
        
        var body = this.Serialize(dto);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-prices/owner", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Domain.Entities.FuelPrice>().Should()
            .Be(FuelPriceCommandControllerData.FuelPriceInitialCount);
    }
    
    [Fact]
    public async Task Fails_to_create_new_fuel_prices_if_user_send_fuel_price_of_fuel_type_that_the_fuel_station_does_not_have()
    {
        // Arrange
        await this.AuthorizeGenericUser(FuelPriceCommandControllerData.FuelStationOwnerUsername, AccountsData.DefaultPassword);
        
        var dto = CreateDto();
        dto.FuelPrices![0].FuelTypeId = FuelPriceCommandControllerData.FuelType3Id;
        
        var body = this.Serialize(dto);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-prices/owner", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Domain.Entities.FuelPrice>().Should()
            .Be(FuelPriceCommandControllerData.FuelPriceInitialCount);
    }
    
    [Fact]
    public async Task Fails_to_create_new_fuel_prices_if_user_is_not_owner_of_fuel_station()
    {
        // Arrange
        await this.AuthorizeGenericUser(FuelPriceCommandControllerData.FuelStationOwnerUsername, AccountsData.DefaultPassword);
        
        var dto = CreateDto();
        dto.FuelStationId = FuelPriceCommandControllerData.NotOwnedFuelStationId;
        
        var body = this.Serialize(dto);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/fuel-prices/owner", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Domain.Entities.FuelPrice>().Should()
            .Be(FuelPriceCommandControllerData.FuelPriceInitialCount);
    }
    
    private NewFuelPricesAtStationDto CreateDto() => new()
    {
        FuelStationId = FuelPriceCommandControllerData.OwnedFuelStationId,
        FuelPrices = new List<NewFuelPriceDto>
        {
            new()
            {
                Available = true,
                Price = 2.0M,
                FuelTypeId = FuelPriceCommandControllerData.FuelType1Id
            },
            new()
            {
                Available = true,
                Price = 3.0M,
                FuelTypeId = FuelPriceCommandControllerData.FuelType2Id
            }
        }
    };
}