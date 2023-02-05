using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Favorites.Commands.DeleteFavourite;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Application.UnitTests.Favourites.Commands.DeleteFavourite;

public class DeleteFavouriteCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IFavoriteRepository> _favoriteRepository;
    private readonly Mock<IFuelStationRepository> _fuelStationRepository;
    private readonly Mock<IUserPrincipalService> _userPrincipalService;
    private readonly DeleteFavouriteCommandHandler _handler;

    public DeleteFavouriteCommandHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _favoriteRepository = new Mock<IFavoriteRepository>();
        _fuelStationRepository = new Mock<IFuelStationRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork
            .Setup(u => u.Users)
            .Returns(_userRepository.Object);
        _unitOfWork
            .Setup(u => u.Favorites)
            .Returns(_favoriteRepository.Object);
        _unitOfWork
            .Setup(u => u.FuelStations)
            .Returns(_fuelStationRepository.Object);
        _userPrincipalService = new Mock<IUserPrincipalService>();

        _handler = new DeleteFavouriteCommandHandler(_unitOfWork.Object, _userPrincipalService.Object);
    }

    [Fact]
    public async Task Deletes_existing_favourite()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;

        var command = new DeleteFavouriteCommand(fuelStationId);
        
        var favourite = new Favorite() { FuelStationId = fuelStationId, UserId = 1 };

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.ExistsByUsername(username))
            .ReturnsAsync(true);

        _fuelStationRepository
            .Setup(x => x.ExistsById(fuelStationId))
            .ReturnsAsync(true);

        _favoriteRepository
            .Setup(x => x.GetByUsernameAndFuelStationIdAsync(username, fuelStationId))
            .ReturnsAsync(favourite);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().Be(Unit.Value);
        
        _favoriteRepository.Verify(x => x.Remove(favourite), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Fails_to_deletes_existing_favourite_for_not_logged_user()
    {
        // Arrange
        const long fuelStationId = 1L;

        var command = new DeleteFavouriteCommand(fuelStationId);
        
        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns((string)null!);

        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<UnauthorizedException>()
            .WithMessage("User is not logged in!");
        
        _favoriteRepository.Verify(x => x.Remove(It.IsAny<Favorite>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Fails_to_deletes_existing_favourite_if_user_not_exists()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;

        var command = new DeleteFavouriteCommand(fuelStationId);
        
        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.ExistsByUsername(username))
            .ReturnsAsync(false);

        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User not found for username = {username}");
        
        _favoriteRepository.Verify(x => x.Remove(It.IsAny<Favorite>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
        
    [Fact]
    public async Task Fails_to_deletes_existing_favourite_if_fuel_station_not_found()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;

        var command = new DeleteFavouriteCommand(fuelStationId);
        
        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.ExistsByUsername(username))
            .ReturnsAsync(true);

        _fuelStationRepository
            .Setup(x => x.ExistsById(fuelStationId))
            .ReturnsAsync(false);

        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Fuel station not found for id = {fuelStationId}");
        
        _favoriteRepository.Verify(x => x.Remove(It.IsAny<Favorite>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Fails_to_deletes_existing_favourite_if_favourite_not_exists()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;

        var command = new DeleteFavouriteCommand(fuelStationId);
        
        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.ExistsByUsername(username))
            .ReturnsAsync(true);

        _fuelStationRepository
            .Setup(x => x.ExistsById(fuelStationId))
            .ReturnsAsync(true);

        _favoriteRepository
            .Setup(x => x.GetByUsernameAndFuelStationIdAsync(username, fuelStationId))
            .ReturnsAsync((Favorite)null!);

        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User with username {username} does not have fuel station with id {fuelStationId} in his favourites");
        
        _favoriteRepository.Verify(x => x.Remove(It.IsAny<Favorite>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
}