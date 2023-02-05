using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models;
using Application.Reviews.Commands.UpdateReview;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.Reviews.Commands.UpdateReview;

public class UpdateReviewCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IReviewRepository> _reviewRepository;
    private readonly Mock<IUserPrincipalService> _userPrincipalService;
    private readonly Mock<IMapper> _mapper;
    private readonly UpdateReviewCommandHandler _handler;

    public UpdateReviewCommandHandlerTest()
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

        _handler = new UpdateReviewCommandHandler(_unitOfWork.Object, _userPrincipalService.Object, _mapper.Object);
    }

    [Fact]
    public async Task Updates_existing_review_if_performed_by_review_author()
    {
        // Arrange
        const long reviewId = 1;
        const long userId = 1;

        var command = new UpdateReviewCommand(reviewId, new UpdateReviewDto { Content = null, Rate = 4 });
        
        var user = new User { Id = userId, Role = Role.User };
        var oldReview = new Review { Rate = 3, Content = "content", UserId = userId };
        var reviewDto = new FuelStationReviewDto { Id = reviewId, Rate = 5, Content = null };

        _reviewRepository
            .Setup(x => x.GetAsync(reviewId))
            .ReturnsAsync(oldReview);

        _userPrincipalService
            .Setup(x => x.GetUserPrincipalId())
            .Returns((int?)userId);

        _userRepository
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync(user);

        _mapper
            .Setup(x => x.Map<FuelStationReviewDto>(It.IsAny<Review>()))
            .Returns(reviewDto);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Id.Should().Be(reviewId);
        
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Updates_existing_review_if_performed_by_admin()
    {
        // Arrange
        const long reviewId = 1;
        const long authorId = 1;
        const long adminId = 2;

        var command = new UpdateReviewCommand(reviewId, new UpdateReviewDto { Content = null, Rate = 4 });
        
        var user = new User { Id = adminId, Role = Role.Admin };
        var oldReview = new Review { Rate = 3, Content = "content", UserId = authorId };
        var reviewDto = new FuelStationReviewDto { Id = reviewId, Rate = 5, Content = null };

        _reviewRepository
            .Setup(x => x.GetAsync(reviewId))
            .ReturnsAsync(oldReview);

        _userPrincipalService
            .Setup(x => x.GetUserPrincipalId())
            .Returns((int?)adminId);

        _userRepository
            .Setup(x => x.GetAsync(adminId))
            .ReturnsAsync(user);

        _mapper
            .Setup(x => x.Map<FuelStationReviewDto>(It.IsAny<Review>()))
            .Returns(reviewDto);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Id.Should().Be(reviewId);
        
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Fails_to_update_review_for_not_existing_review()
    {
        // Arrange
        const long reviewId = 1;

        var command = new UpdateReviewCommand(reviewId, new UpdateReviewDto { Content = "content", Rate = 4 });

        _reviewRepository
            .Setup(x => x.GetAsync(reviewId))
            .ReturnsAsync((Review)null!);
        
        // Act
        Func<Task<FuelStationReviewDto>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Review not found for id = {reviewId}");
        
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Fails_to_update_review_for_not_logged_user()
    {
        // Arrange
        const long reviewId = 1;
        const long userId = 1;

        var command = new UpdateReviewCommand(reviewId, new UpdateReviewDto { Content = "content", Rate = 4 });
        var oldReview = new Review { Rate = 3, Content = "content", UserId = userId };

        _reviewRepository
            .Setup(x => x.GetAsync(reviewId))
            .ReturnsAsync(oldReview);

        _userPrincipalService
            .Setup(x => x.GetUserPrincipalId())
            .Returns((int?)null);
        
        // Act
        Func<Task<FuelStationReviewDto>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<UnauthorizedException>()
            .WithMessage("User is not logged in!");
        
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Fails_to_update_review_if_could_not_found_user_by_id()
    {
        // Arrange
        const long reviewId = 1;
        const long userId = 1;

        var command = new UpdateReviewCommand(reviewId, new UpdateReviewDto { Content = "content", Rate = 4 });
        var oldReview = new Review { Rate = 3, Content = "content", UserId = userId };

        _reviewRepository
            .Setup(x => x.GetAsync(reviewId))
            .ReturnsAsync(oldReview);

        _userPrincipalService
            .Setup(x => x.GetUserPrincipalId())
            .Returns((int?)userId);

        _userRepository
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync((User)null!);
        
        // Act
        Func<Task<FuelStationReviewDto>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User not found for id = {userId}");
        
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Fails_to_update_review_if_user_is_not_admin_and_author()
    {
        // Arrange
        const long reviewId = 1;
        const long authorId = 2;
        const long userId = 1;

        var command = new UpdateReviewCommand(reviewId, new UpdateReviewDto { Content = "content", Rate = 4 });
        var oldReview = new Review { Rate = 3, Content = "content", UserId = authorId };
        var user = new User { Id = userId, Username = "User", Role = Role.User };

        _reviewRepository
            .Setup(x => x.GetAsync(reviewId))
            .ReturnsAsync(oldReview);

        _userPrincipalService
            .Setup(x => x.GetUserPrincipalId())
            .Returns((int?)userId);

        _userRepository
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync(user);
        
        // Act
        Func<Task<FuelStationReviewDto>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<ForbiddenException>()
            .WithMessage($"User with username {user.Username} does not have permission to edit this review");
        
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
}