using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Models;
using Application.Reviews.Queries.GetUserReviewOfFuelStation;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.Reviews.Queries.GetUserReviewOfFuelStation;

public class GetUserReviewOfFuelStationQueryHandlerTest
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IFuelStationRepository> _fuelStationRepository;
    private readonly Mock<IReviewRepository> _reviewRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly GetUserReviewOfFuelStationQueryHandler _handler; 
    
    public GetUserReviewOfFuelStationQueryHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _fuelStationRepository = new Mock<IFuelStationRepository>();
        _reviewRepository = new Mock<IReviewRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork
            .Setup(u => u.Users)
            .Returns(_userRepository.Object);
        unitOfWork
            .Setup(u => u.FuelStations)
            .Returns(_fuelStationRepository.Object);
        unitOfWork
            .Setup(u => u.Reviews)
            .Returns(_reviewRepository.Object);
        _mapper = new Mock<IMapper>();

        _handler = new GetUserReviewOfFuelStationQueryHandler(unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task Returns_review()
    {
        // Arrange
        const long fuelStationId = 1L;
        const string username = "User";

        var query = new GetUserReviewOfFuelStationQuery(username, fuelStationId);

        var review = new Review { Rate = 1, Content = null, UserId = 1, FuelStationId = fuelStationId };
        var reviewDto = new FuelStationReviewDto { Rate = 1, Content = null, Username = username, UserId = 1, FuelStationId = fuelStationId };
        
        _userRepository
            .Setup(x => x.ExistsByUsername(username))
            .ReturnsAsync(true);

        _fuelStationRepository
            .Setup(x => x.ExistsById(fuelStationId))
            .ReturnsAsync(true);

        _reviewRepository
            .Setup(x => x.GetByFuelStationAndUsername(fuelStationId, username))
            .ReturnsAsync(review);

        _mapper
            .Setup(x => x.Map<FuelStationReviewDto>(review))
            .Returns(reviewDto);
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Fails_to_return_review_for_not_existing_user()
    {
        // Arrange
        const string username = "User";

        var query = new GetUserReviewOfFuelStationQuery(username, 1);

        _userRepository
            .Setup(x => x.ExistsByUsername(username))
            .ReturnsAsync(false);
        
        // Act
        Func<Task<FuelStationReviewDto>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User not found for username = {username}");
    }

    [Fact]
    public async Task Fails_to_return_review_for_not_existing_fuel_station()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1;

        var query = new GetUserReviewOfFuelStationQuery(username, fuelStationId);

        _userRepository
            .Setup(x => x.ExistsByUsername(username))
            .ReturnsAsync(true);

        _fuelStationRepository
            .Setup(x => x.ExistsById(fuelStationId))
            .ReturnsAsync(false);
        
        // Act
        Func<Task<FuelStationReviewDto>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Fuel station not found for id = {fuelStationId}");
    }

    [Fact]
    public async Task Fails_to_return_review_if_review_not_exists_for_given_username_and_fuel_station()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1;

        var query = new GetUserReviewOfFuelStationQuery(username, fuelStationId);

        _userRepository
            .Setup(x => x.ExistsByUsername(username))
            .ReturnsAsync(true);

        _fuelStationRepository
            .Setup(x => x.ExistsById(fuelStationId))
            .ReturnsAsync(true);

        _reviewRepository
            .Setup(x => x.GetByFuelStationAndUsername(fuelStationId, username))
            .ReturnsAsync((Review)null!);
        
        // Act
        Func<Task<FuelStationReviewDto>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Review not found for username = {username} and fuel station id = {fuelStationId}");
    }
}