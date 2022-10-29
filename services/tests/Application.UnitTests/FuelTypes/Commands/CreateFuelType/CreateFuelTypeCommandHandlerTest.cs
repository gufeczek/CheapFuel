using System.Threading;
using System.Threading.Tasks;
using Application.FuelTypes.Commands.CreateFuelType;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.FuelTypes.Commands.CreateFuelType;

public class CreateFuelTypeCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IFuelTypeRepository> _fuelTypeRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly CreateFuelTypeCommandHandler _handler;

    public CreateFuelTypeCommandHandlerTest()
    {
        _fuelTypeRepository = new Mock<IFuelTypeRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork
            .Setup(u => u.FuelTypes)
            .Returns(_fuelTypeRepository.Object);
        _mapper = new Mock<IMapper>();

        _handler = new CreateFuelTypeCommandHandler(_unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task Creates_new_fuel_type()
    {
        // Arrange
        const string name = "ON";
        const long id = 1;

        var command = new CreateFuelTypeCommand(name);
        var fuelTypeDto = new FuelTypeDto(id, name);

        _mapper
            .Setup(x => x.Map<FuelTypeDto>(It.IsAny<FuelType>()))
            .Returns(fuelTypeDto);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Id.Should().Be(id);
        result.Name.Should().Be(name);
        
        _fuelTypeRepository.Verify(x => x.Add(It.IsAny<FuelType>()), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }
}