using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.FuelTypes.Commands.DeleteFuelType;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Application.UnitTests.FuelTypes.Commands.DeleteFuelType;

public class DeleteFuelTypeCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IFuelTypeRepository> _fuelTypeRepository;
    private readonly DeleteFuelTypeCommandHandler _handler;

    public DeleteFuelTypeCommandHandlerTest()
    {
        _fuelTypeRepository = new Mock<IFuelTypeRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork
            .Setup(u => u.FuelTypes)
            .Returns(_fuelTypeRepository.Object);

        _handler = new DeleteFuelTypeCommandHandler(_unitOfWork.Object);
    }

    [Fact]
    public async Task Deletes_existing_fuel_type()
    {
        // Arrange
        const int id = 1;

        DeleteFuelTypeCommand command = new(id);
        FuelType fuelType = new() { Name = "NO" };

        _fuelTypeRepository
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync(fuelType);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().Be(Unit.Value);
        
        _fuelTypeRepository.Verify(x => x.Remove(fuelType), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Throws_exception_for_not_existing_fuel_type()
    {
        // Arrange
        const int id = 1;

        DeleteFuelTypeCommand command = new(id);

        _fuelTypeRepository
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync((FuelType)null!);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Fuel type not found for id = {id}");
        
        _fuelTypeRepository.Verify(x => x.Remove(It.IsAny<FuelType>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Throws_exception_if_id_equals_null()
    {
        // Arrange
        int? id = null;

        DeleteFuelTypeCommand command = new(id);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<ArgumentNullException>();
        
        _fuelTypeRepository.Verify(x => x.GetAsync(It.IsAny<long>()), Times.Never);
        _fuelTypeRepository.Verify(x => x.Remove(It.IsAny<FuelType>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
}