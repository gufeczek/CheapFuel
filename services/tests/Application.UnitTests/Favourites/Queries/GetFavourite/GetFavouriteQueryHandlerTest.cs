using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Favorites.Queries.GetFavourite;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.Favourites.Queries.GetFavourite;

public class GetFavouriteQueryHandlerTest
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IFavoriteRepository> _favoriteRepository;
    private readonly Mock<IFuelStationRepository> _fuelStationRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly GetFavouriteQueryHandler _handler;
    
    public GetFavouriteQueryHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _favoriteRepository = new Mock<IFavoriteRepository>();
        _fuelStationRepository = new Mock<IFuelStationRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork
            .Setup(u => u.Users)
            .Returns(_userRepository.Object);
        unitOfWork
            .Setup(u => u.Favorites)
            .Returns(_favoriteRepository.Object);
        unitOfWork
            .Setup(u => u.FuelStations)
            .Returns(_fuelStationRepository.Object);
        _mapper = new Mock<IMapper>();

        _handler = new GetFavouriteQueryHandler(unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task Returns_favourite()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;

        var query = new GetFavouriteQuery(username, fuelStationId);

        var favourite = new Favorite { FuelStationId = fuelStationId, UserId = 1 };
        var favouriteDto = new SimpleUserFavouriteDto { FuelStationId = fuelStationId, Username = username };

        _userRepository
            .Setup(x => x.ExistsByUsername(username))
            .ReturnsAsync(true);

        _fuelStationRepository
            .Setup(x => x.ExistsById(fuelStationId))
            .ReturnsAsync(true);

        _favoriteRepository
            .Setup(x => x.GetByUsernameAndFuelStationIdAsync(username, fuelStationId))
            .ReturnsAsync(favourite);

        _mapper
            .Setup(x => x.Map<SimpleUserFavouriteDto>(favourite))
            .Returns(favouriteDto);
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Fails_to_return_favourite_if_user_not_exists()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;

        var query = new GetFavouriteQuery(username, fuelStationId);
        
        _userRepository
            .Setup(x => x.ExistsByUsername(username))
            .ReturnsAsync(false);

        // Act
        Func<Task<SimpleUserFavouriteDto>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User not found for username = {username}");
        
        _favoriteRepository.Verify(x => x.GetByUsernameAndFuelStationIdAsync(It.IsAny<string>(), It.IsAny<long>()), Times.Never);
    }
    
    [Fact]
    public async Task Fails_to_return_favourite_if_fuel_station_not_exists()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;

        var query = new GetFavouriteQuery(username, fuelStationId);
        
        _userRepository
            .Setup(x => x.ExistsByUsername(username))
            .ReturnsAsync(true);
        
        _fuelStationRepository
            .Setup(x => x.ExistsById(fuelStationId))
            .ReturnsAsync(false);

        // Act
        Func<Task<SimpleUserFavouriteDto>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Fuel station not found for id = {fuelStationId}");
        
        _favoriteRepository.Verify(x => x.GetByUsernameAndFuelStationIdAsync(It.IsAny<string>(), It.IsAny<long>()), Times.Never);
    }
    
    [Fact]
    public async Task Fails_to_return_favourite_if_favourite_not_exists()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;

        var query = new GetFavouriteQuery(username, fuelStationId);
        
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
        Func<Task<SimpleUserFavouriteDto>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User with username {username} does not have fuel station with id {fuelStationId} in his favourites");
    }
}