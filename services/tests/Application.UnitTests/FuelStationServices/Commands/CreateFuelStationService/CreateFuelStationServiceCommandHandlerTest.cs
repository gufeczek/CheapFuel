using System.Threading;
using System.Threading.Tasks;
using Application.FuelStationServices.Commands.CreateFuelStationService;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.FuelStationServices.Commands.CreateFuelStationService;

public class CreateFuelStationServiceCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IFuelStationServiceRepository> _serviceRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly CreateFuelStationServiceCommandHandler _handler;

    public CreateFuelStationServiceCommandHandlerTest()
    {
        _serviceRepository = new Mock<IFuelStationServiceRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork
            .Setup(u => u.Services)
            .Returns(_serviceRepository.Object);
        _mapper = new Mock<IMapper>();
        
        _handler = new CreateFuelStationServiceCommandHandler(_unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task Creates_new_fuel_station_service()
    {
        // Arrange
        const string name = "Service 1";
        const long id = 1;
        
        var command = new CreateFuelStationServiceCommand(name);
        var serviceDto = new FuelStationServiceDto { Id = id, Name = name };

        _mapper
            .Setup(x => x.Map<FuelStationServiceDto>(It.IsAny<FuelStationService>()))
            .Returns(serviceDto);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Id.Should().Be(id);
        result.Name.Should().Be(name);
        
        _serviceRepository.Verify(x => x.Add(It.IsAny<FuelStationService>()), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }
}