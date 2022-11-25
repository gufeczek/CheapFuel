using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models;
using Application.Reviews.Commands.CreateReview;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.Reviews.Commands.CreateReview;

public class CreateReviewCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IReviewRepository> _reviewRepository;
    private readonly Mock<IUserPrincipalService> _userPrincipalService;
    private readonly Mock<IMapper> _mapper;
    private readonly CreateReviewCommandHandler _handler;
    
    public CreateReviewCommandHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _reviewRepository = new Mock<IReviewRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork
            .Setup(u => u.Users)
            .Returns(_userRepository.Object);
        _unitOfWork
            .Setup(u => u.Reviews)
            .Returns(_reviewRepository.Object);
        _userPrincipalService = new Mock<IUserPrincipalService>();
        _mapper = new Mock<IMapper>();

        _handler = new CreateReviewCommandHandler(_unitOfWork.Object, _userPrincipalService.Object, _mapper.Object);
    }

    [Fact]
    public async Task Creates_new_review()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;

        var command = new CreateReviewCommand(5, null, fuelStationId);
        
        var user = new User { Id = 1, Username = username };
        var reviewDto = new FuelStationReviewDto { Id = 1, Rate = 5, Content = null, Username = username };
        
        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        _reviewRepository
            .Setup(x => x.ExistsByFuelStationAndUsername(fuelStationId, username))
            .ReturnsAsync(false);

        _mapper
            .Setup(x => x.Map<FuelStationReviewDto>(It.IsAny<Review>()))
            .Returns(reviewDto);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Id.Should().Be(1);
        result.Username.Should().Be(username);
        
        _reviewRepository.Verify(x => x.Add(It.IsAny<Review>()), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Fails_to_create_review_for_not_logged_user()
    {
        // Arrange
        var command = new CreateReviewCommand(5, null, 1);
        
        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns((string)null!);
        
        // Act
        Func<Task<FuelStationReviewDto>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<UnauthorizedException>()
            .WithMessage("User is not logged in!");
        
        _reviewRepository.Verify(x => x.Add(It.IsAny<Review>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Fails_to_create_review_if_could_not_found_user_by_username()
    {
        // Arrange
        const string username = "User";
        
        var command = new CreateReviewCommand(5, null, 1);
        
        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync((User)null!);
        
        // Act
        Func<Task<FuelStationReviewDto>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User not found for username = {username}");
        
        _reviewRepository.Verify(x => x.Add(It.IsAny<Review>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Fails_to_create_review_if_user_already_rated_given_fuel_station()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;

        var command = new CreateReviewCommand(5, null, fuelStationId);
        
        var user = new User { Id = 1, Username = username };

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        _reviewRepository
            .Setup(x => x.ExistsByFuelStationAndUsername(fuelStationId, username))
            .ReturnsAsync(true);
        
        // Act
        Func<Task<FuelStationReviewDto>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<ConflictException>()
            .WithMessage($"User with username = {username} has already left a review for fuel station with id = {fuelStationId}");
        
        _reviewRepository.Verify(x => x.Add(It.IsAny<Review>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
}