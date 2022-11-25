using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Favorites.Commands.CreateFavourite;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.Favourites.Commands.CreateFavourite;

public class CreateFavouriteCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IFavoriteRepository> _favoriteRepository;
    private readonly Mock<IFuelStationRepository> _fuelStationRepository;
    private readonly Mock<IUserPrincipalService> _userPrincipalService;
    private readonly Mock<IMapper> _mapper;
    private readonly CreateFavouriteCommandHandler _handler;
    
    public CreateFavouriteCommandHandlerTest()
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
        _mapper = new Mock<IMapper>();

        _handler = new CreateFavouriteCommandHandler(_unitOfWork.Object, _userPrincipalService.Object, _mapper.Object);
    }

    [Fact]
    public async Task Creates_new_favourite()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;

        var command = new CreateFavouriteCommand(fuelStationId);

        var user = new User { Id = 1, Username = username };
        var fuelStation = new FuelStation { Id = fuelStationId };
        var favouriteDto = new SimpleUserFavouriteDto { FuelStationId = fuelStationId, Username = username };

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _favoriteRepository
            .Setup(x => x.ExistsByUsernameAndFuelStationIdAsync(username, fuelStationId))
            .ReturnsAsync(false);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        _fuelStationRepository
            .Setup(x => x.GetAsync(fuelStationId))
            .ReturnsAsync(fuelStation);

        _mapper
            .Setup(x => x.Map<SimpleUserFavouriteDto>(It.IsAny<Favorite>()))
            .Returns(favouriteDto);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        
        _favoriteRepository.Verify(x => x.Add(It.IsAny<Favorite>()), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Fails_to_create_favourite_for_not_logged_user()
    {
        // Arrange
        const long fuelStationId = 1L;
        
        
        var command = new CreateFavouriteCommand(fuelStationId);
        
        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns((string)null!);

        // Act
        Func<Task<SimpleUserFavouriteDto>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<UnauthorizedException>()
            .WithMessage("User is not logged in!");
        
        _favoriteRepository.Verify(x => x.Add(It.IsAny<Favorite>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Fails_to_create_favourite_if_fuel_station_is_already_added_to_user_favourites()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;
        
        var command = new CreateFavouriteCommand(fuelStationId);
        
        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _favoriteRepository
            .Setup(x => x.ExistsByUsernameAndFuelStationIdAsync(username, fuelStationId))
            .ReturnsAsync(true);

        // Act
        Func<Task<SimpleUserFavouriteDto>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<ConflictException>()
            .WithMessage($"User with username = {username} has already added fuel station with id = {fuelStationId} to his favourites");
        
        _favoriteRepository.Verify(x => x.Add(It.IsAny<Favorite>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Fails_to_create_favourite_if_user_not_found()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;
        
        var command = new CreateFavouriteCommand(fuelStationId);
        
        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _favoriteRepository
            .Setup(x => x.ExistsByUsernameAndFuelStationIdAsync(username, fuelStationId))
            .ReturnsAsync(false);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync((User)null!);

        // Act
        Func<Task<SimpleUserFavouriteDto>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User not found for username = {username}");
        
        _favoriteRepository.Verify(x => x.Add(It.IsAny<Favorite>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Fails_to_create_favourite_if_fuel_station_not_found()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;
        
        var command = new CreateFavouriteCommand(fuelStationId);
        
        var user = new User { Id = 1, Username = username };

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _favoriteRepository
            .Setup(x => x.ExistsByUsernameAndFuelStationIdAsync(username, fuelStationId))
            .ReturnsAsync(false);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        _fuelStationRepository
            .Setup(x => x.GetAsync(fuelStationId))
            .ReturnsAsync((FuelStation)null!);

        // Act
        Func<Task<SimpleUserFavouriteDto>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Fuel station not found for id = {fuelStationId}");
        
        _favoriteRepository.Verify(x => x.Add(It.IsAny<Favorite>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
}