using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.FuelStationServices.Commands.DeleteFuelStationService;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Application.UnitTests.FuelStationServices.Commands.DeleteFuelStationService;

public class DeleteFuelStationServiceCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IFuelStationServiceRepository> _serviceRepository;
    private readonly DeleteFuelStationServiceCommandHandler _handler;

    public DeleteFuelStationServiceCommandHandlerTest()
    {
        _serviceRepository = new Mock<IFuelStationServiceRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork
            .Setup(u => u.Services)
            .Returns(_serviceRepository.Object);

        _handler = new DeleteFuelStationServiceCommandHandler(_unitOfWork.Object);
    }

    [Fact]
    public async Task Deletes_existing_fuel_station_service()
    {
        // Arrange
        const int id = 1;

        DeleteFuelStationServiceCommand command = new(id);
        FuelStationService? service = new() { Name = "Service 1" };

        _serviceRepository
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync(service);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().Be(Unit.Value);
        
        _serviceRepository.Verify(x => x.Remove(service), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Throws_exception_for_not_existing_fuel_station_service()
    {
        // Arrange
        const int id = 1;
        
        DeleteFuelStationServiceCommand command = new(id);

        _serviceRepository
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync((FuelStationService)null!);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Service not found for id = {id}");
        
        _serviceRepository.Verify(x => x.Remove(It.IsAny<FuelStationService>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Throws_exception_if_id_equals_null()
    {
        // Arrange
        int? id = null;
        
        DeleteFuelStationServiceCommand command = new(id);

        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<ArgumentNullException>();
        
        _serviceRepository.Verify(x => x.GetAsync(It.IsAny<long>()), Times.Never);
        _serviceRepository.Verify(x => x.Remove(It.IsAny<FuelStationService>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
}