using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.StationChains.Commands.CreateStationChain;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.StationChains.Commands.CreateStationChain;

public class CreateStationChainCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IStationChainRepository> _stationChainRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly CreateStationChainCommandHandler _handler;

    public CreateStationChainCommandHandlerTest()
    {
        _stationChainRepository = new Mock<IStationChainRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork
            .Setup(u => u.StationChains)
            .Returns(_stationChainRepository.Object);
        _mapper = new Mock<IMapper>();
        
        _handler = new CreateStationChainCommandHandler(_unitOfWork.Object, _mapper.Object);
    }
    
    [Fact]
    public async Task Creates_new_station_chain()
    {
        // Arrange
        const string name = "Lotos";
        const long id = 1;
        
        var command = new CreateStationChainCommand(name);
        var stationChainDto = new StationChainDto(id, name);

        _mapper
            .Setup(x => x.Map<StationChainDto>(It.IsAny<StationChain>()))
            .Returns(stationChainDto);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Id.Should().Be(id);
        result.Name.Should().Be(name);
        
        _stationChainRepository.Verify(x => x.Add(It.IsAny<StationChain>()), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }
}