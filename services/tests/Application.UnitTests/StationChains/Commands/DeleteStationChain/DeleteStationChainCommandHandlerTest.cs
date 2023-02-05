using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.StationChains.Commands.DeleteStationChain;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Application.UnitTests.StationChains.Commands.DeleteStationChain;

public class DeleteStationChainCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IStationChainRepository> _stationChainRepository;
    private readonly DeleteStationChainCommandHandler _handler;

    public DeleteStationChainCommandHandlerTest()
    {
        _stationChainRepository = new Mock<IStationChainRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork
            .Setup(u => u.StationChains)
            .Returns(_stationChainRepository.Object);

        _handler = new DeleteStationChainCommandHandler(_unitOfWork.Object);
    }
    
    [Fact]
    public async Task Deletes_existing_station_chain()
    {
        // Arrange
        const int id = 1;

        DeleteStationChainCommand command = new(id);
        StationChain? stationChain = new() { Name = "Lotos" };

        _stationChainRepository
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync(stationChain);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().Be(Unit.Value);
        
        _stationChainRepository.Verify(x => x.Remove(stationChain), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Throws_exception_for_not_existing_station_chain()
    {
        // Arrange
        const int id = 1;
        
        DeleteStationChainCommand command = new(id);

        _stationChainRepository
            .Setup(x => x.GetAsync(id))
            .ReturnsAsync((StationChain)null!);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Station chain not found for if = {id}");
        
        _stationChainRepository.Verify(x => x.Remove(It.IsAny<StationChain>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Throws_exception_if_id_equals_null()
    {
        // Arrange
        int? id = null;
        
        DeleteStationChainCommand command = new(id);

        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<ArgumentNullException>();
        
        _stationChainRepository.Verify(x => x.GetAsync(It.IsAny<long>()), Times.Never);
        _stationChainRepository.Verify(x => x.Remove(It.IsAny<StationChain>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
}