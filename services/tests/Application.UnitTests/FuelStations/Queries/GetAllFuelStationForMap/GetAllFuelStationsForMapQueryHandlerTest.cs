using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.FuelStations.Queries.GetAllFuelStationForMap;
using Application.Models;
using Application.Models.Filters;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.FuelStations.Queries.GetAllFuelStationForMap;

public class GetAllFuelStationsForMapQueryHandlerTest
{
    private readonly Mock<IFuelStationRepository> _fuelStationRepository;
    private readonly Mock<IFuelTypeRepository> _fuelTypeRepository;
    private readonly Mock<IFuelStationServiceRepository> _fuelStationServiceRepository;
    private readonly Mock<IStationChainRepository> _stationChainRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly GetAllFuelStationsForMapQueryHandler _handler;
    
    public GetAllFuelStationsForMapQueryHandlerTest()
    {
        _fuelStationRepository = new Mock<IFuelStationRepository>();
        _fuelTypeRepository = new Mock<IFuelTypeRepository>();
        _fuelStationServiceRepository = new Mock<IFuelStationServiceRepository>();
        _stationChainRepository = new Mock<IStationChainRepository>();
        
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork
            .Setup(u => u.FuelStations)
            .Returns(_fuelStationRepository.Object);
        unitOfWork
            .Setup(u => u.FuelTypes)
            .Returns(_fuelTypeRepository.Object);
        unitOfWork
            .Setup(u => u.Services)
            .Returns(_fuelStationServiceRepository.Object);
        unitOfWork
            .Setup(u => u.StationChains)
            .Returns(_stationChainRepository.Object);

        _mapper = new Mock<IMapper>();

        _handler = new GetAllFuelStationsForMapQueryHandler(unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task Returns_list_of_all_fuel_stations()
    {
        // Arrange
        var filter = new FuelStationFilterDto(
            FuelTypeId: 1,
            ServicesIds: new List<long> { 1, 2 },
            StationChainsIds: new List<long> { 1, 2 },
            MinPrice: 1.0M,
            MaxPrice: 2.0M);

        var query = new GetAllFuelStationsForMapQuery(filter);

        _fuelTypeRepository
            .Setup(x => x.ExistsById(filter.FuelTypeId))
            .ReturnsAsync(true);

        _stationChainRepository
            .Setup(x => x.ExistsAllById(filter.StationChainsIds!))
            .ReturnsAsync(true);
        
        _fuelStationServiceRepository
            .Setup(x => x.ExistsAllById(filter.ServicesIds!))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        
        _fuelStationRepository
            .Verify(x => 
                x.GetFuelStationsWithFuelPrice(
                    filter.FuelTypeId, 
                    filter.ServicesIds, 
                    filter.StationChainsIds, 
                    filter.MinPrice, 
                    filter.MaxPrice), 
                Times.Once);
        
        _mapper.Verify(x => 
            x.Map<IEnumerable<SimpleFuelStationDto>>(It.IsAny<IEnumerable<FuelStation>>()), 
            Times.Once);
    }

    [Fact]
    public async Task Fails_to_fetch_fuel_stations_for_invalid_fuel_type_id()
    {
        // Arrange
        var filter = new FuelStationFilterDto(
            FuelTypeId: 1,
            ServicesIds: new List<long> { 1, 2 },
            StationChainsIds: new List<long> { 1, 2 },
            MinPrice: 1.0M,
            MaxPrice: 2.0M);

        var query = new GetAllFuelStationsForMapQuery(filter);
        
        _fuelTypeRepository
            .Setup(x => x.ExistsById(filter.FuelTypeId))
            .ReturnsAsync(false);
        
        // Act
        Func<Task<IEnumerable<SimpleFuelStationDto>>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<FilterValidationException>();
        
        _fuelStationRepository
            .Verify(x => 
                    x.GetFuelStationsWithFuelPrice(
                        It.IsAny<long>(), 
                        It.IsAny<IEnumerable<long>?>(), 
                        It.IsAny<IEnumerable<long>?>(), 
                        It.IsAny<decimal?>(), It.IsAny<decimal?>()), 
                Times.Never);
        
        _mapper.Verify(x => 
                x.Map<IEnumerable<SimpleFuelStationDto>>(It.IsAny<IEnumerable<FuelStation>>()), 
            Times.Never);
    }
    
    [Fact]
    public async Task Fails_to_fetch_fuel_stations_for_at_least_one_invalid_station_chain_id()
    {
        // Arrange
        var filter = new FuelStationFilterDto(
            FuelTypeId: 1,
            ServicesIds: new List<long> { 1, 2 },
            StationChainsIds: new List<long> { 1, 2 },
            MinPrice: 1.0M,
            MaxPrice: 2.0M);

        var query = new GetAllFuelStationsForMapQuery(filter);
        
        _fuelTypeRepository
            .Setup(x => x.ExistsById(filter.FuelTypeId))
            .ReturnsAsync(true);
        
        _stationChainRepository
            .Setup(x => x.ExistsAllById(filter.StationChainsIds!))
            .ReturnsAsync(false);
        
        // Act
        Func<Task<IEnumerable<SimpleFuelStationDto>>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<FilterValidationException>();
        
        _fuelStationRepository
            .Verify(x => 
                    x.GetFuelStationsWithFuelPrice(
                        It.IsAny<long>(), 
                        It.IsAny<IEnumerable<long>?>(), 
                        It.IsAny<IEnumerable<long>?>(), 
                        It.IsAny<decimal?>(), It.IsAny<decimal?>()), 
                Times.Never);
        
        _mapper.Verify(x => 
                x.Map<IEnumerable<SimpleFuelStationDto>>(It.IsAny<IEnumerable<FuelStation>>()), 
            Times.Never);
    }
    
    [Fact]
    public async Task Fails_to_fetch_fuel_stations_for_at_least_one_invalid_service_id()
    {
        // Arrange
        var filter = new FuelStationFilterDto(
            FuelTypeId: 1,
            ServicesIds: new List<long> { 1, 2 },
            StationChainsIds: new List<long> { 1, 2 },
            MinPrice: 1.0M,
            MaxPrice: 2.0M);

        var query = new GetAllFuelStationsForMapQuery(filter);
        
        _fuelTypeRepository
            .Setup(x => x.ExistsById(filter.FuelTypeId))
            .ReturnsAsync(true);
        
        _stationChainRepository
            .Setup(x => x.ExistsAllById(filter.StationChainsIds!))
            .ReturnsAsync(true);
        
        _fuelStationServiceRepository
            .Setup(x => x.ExistsAllById(filter.ServicesIds!))
            .ReturnsAsync(false);
        
        // Act
        Func<Task<IEnumerable<SimpleFuelStationDto>>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<FilterValidationException>();
        
        _fuelStationRepository
            .Verify(x => 
                    x.GetFuelStationsWithFuelPrice(
                        It.IsAny<long>(), 
                        It.IsAny<IEnumerable<long>?>(), 
                        It.IsAny<IEnumerable<long>?>(), 
                        It.IsAny<decimal?>(), It.IsAny<decimal?>()), 
                Times.Never);
        
        _mapper.Verify(x => 
                x.Map<IEnumerable<SimpleFuelStationDto>>(It.IsAny<IEnumerable<FuelStation>>()), 
            Times.Never);
    }
}