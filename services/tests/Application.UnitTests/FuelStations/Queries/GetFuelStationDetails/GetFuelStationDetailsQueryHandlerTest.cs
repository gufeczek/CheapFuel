using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.FuelStations.Queries.GetFuelStationDetails;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.FuelStations.Queries.GetFuelStationDetails;

public class GetFuelStationDetailsQueryHandlerTest
{
    private readonly Mock<IFuelStationRepository> _fuelStationRepository;
    private readonly Mock<IFuelPriceRepository> _fuelPriceRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly GetFuelStationDetailsQueryHandler _handler;

    public GetFuelStationDetailsQueryHandlerTest()
    {
        _fuelStationRepository = new Mock<IFuelStationRepository>();
        _fuelPriceRepository = new Mock<IFuelPriceRepository>();

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork
            .Setup(u => u.FuelStations)
            .Returns(_fuelStationRepository.Object);
        unitOfWork
            .Setup(u => u.FuelPrices)
            .Returns(_fuelPriceRepository.Object);

        _mapper = new Mock<IMapper>();

        _handler = new GetFuelStationDetailsQueryHandler(unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task Returns_fuel_station_details()
    {
        // Arrange
        const int fuelStationId = 1;

        var query = new GetFuelStationDetailsQuery(fuelStationId);

        var fuelStation = GetFuelStation();
        var fuelPrice = GetFuelPrice();

        _fuelStationRepository
            .Setup(x => x.GetFuelStationWithAllDetailsAsync(fuelStationId))
            .ReturnsAsync(fuelStation);

        _fuelPriceRepository
            .Setup(x => x.GetMostRecentPrice(fuelStationId, 1))
            .ReturnsAsync(fuelPrice);

        _fuelPriceRepository
            .Setup(x => x.GetMostRecentPrice(fuelStationId, 2))
            .ReturnsAsync((FuelPrice)null!);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(fuelStationId);
        
        _mapper.Verify(x => x.Map<AddressDto>(It.IsAny<object>()), Times.Once);
        _mapper.Verify(x => x.Map<StationChainDto>(It.IsAny<object>()), Times.Once);
        _mapper.Verify(x => x.Map<IEnumerable<OpeningClosingTimeDto>>(It.IsAny<object>()), Times.Once);
        _mapper.Verify(x => x.Map<IEnumerable<FuelStationServiceDto>>(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task Fails_to_fetch_fuel_station_for_not_existsing_fuel_station()
    {
        // Arrange
        const int fuelStationId = 1;

        var query = new GetFuelStationDetailsQuery(fuelStationId);

        _fuelStationRepository
            .Setup(x => x.GetFuelStationWithAllDetailsAsync(fuelStationId))
            .ReturnsAsync((FuelStation)null!);
        
        // Act
        Func<Task<FuelStationDetailsDto>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Fuel station not found for id = {fuelStationId}");
    }

    private FuelStation GetFuelStation()
    {
        return new FuelStation
        {
            Id = 1,
            Name = "Fuel station 1",
            Address = new Address
            {
                City = "Lublin",
                PostalCode = "20388",
                Street = "Bursztynowa",
                StreetNumber = "1"
            },
            StationChain = new StationChain
            {
                Id = 1,
                Name = "Orlen"
            }
        };
    }

    private FuelPrice GetFuelPrice()
    {
        return new FuelPrice
        {
            Available = true,
            FuelStationId = 1,
            FuelTypeId = 1,
            Price = 2.50M,
            Status = FuelPriceStatus.Accepted
        };
    }
}