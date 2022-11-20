using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Reviews.Commands.DeleteReview;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Application.UnitTests.Reviews.Commands.DeleteReview;

public class DeleteReviewCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IReviewRepository> _reviewRepository;
    private readonly Mock<IUserPrincipalService> _userPrincipalService;
    private readonly DeleteReviewCommandHandler _handler;

    public DeleteReviewCommandHandlerTest()
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

        _handler = new DeleteReviewCommandHandler(_unitOfWork.Object, _userPrincipalService.Object);
    }

    [Fact]
    public async Task Deletes_existing_review_if_performed_by_review_author()
    {
        // Arrange
        const long reviewId = 1;
        const long userId = 1;

        var command = new DeleteReviewCommand(reviewId);
        
        var user = new User { Id = userId, Role = Role.User };
        var review = new Review { Rate = 3, Content = "content", UserId = userId };

        _reviewRepository
            .Setup(x => x.GetAsync(reviewId))
            .ReturnsAsync(review);

        _userPrincipalService
            .Setup(x => x.GetUserPrincipalId())
            .Returns((int?)userId);

        _userRepository
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().Be(Unit.Value);
        
        _reviewRepository.Verify(x => x.Remove(review), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Deletes_existing_review_if_performed_by_admin()
    {
        // Arrange
        const long reviewId = 1;
        const long adminId = 2;
        const long authorId = 1;

        var command = new DeleteReviewCommand(reviewId);
        
        var user = new User { Id = adminId, Role = Role.Admin };
        var review = new Review { Rate = 3, Content = "content", UserId = authorId };

        _reviewRepository
            .Setup(x => x.GetAsync(reviewId))
            .ReturnsAsync(review);

        _userPrincipalService
            .Setup(x => x.GetUserPrincipalId())
            .Returns((int?)adminId);

        _userRepository
            .Setup(x => x.GetAsync(adminId))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().Be(Unit.Value);
        
        _reviewRepository.Verify(x => x.Remove(review), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Fails_to_delete_review_for_not_existing_review()
    {
        // Arrange
        const long reviewId = 1;

        var command = new DeleteReviewCommand(reviewId);

        _reviewRepository
            .Setup(x => x.GetAsync(reviewId))
            .ReturnsAsync((Review)null!);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Review not found for id = {reviewId}");
        
        _reviewRepository.Verify(x => x.Remove(It.IsAny<Review>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Fails_to_delete_review_for_not_logged_user()
    {
        // Arrange
        const long reviewId = 1;
        const long userId = 1;

        var command = new DeleteReviewCommand(reviewId);
        var review = new Review { Rate = 3, Content = "content", UserId = userId };

        _reviewRepository
            .Setup(x => x.GetAsync(reviewId))
            .ReturnsAsync(review);

        _userPrincipalService
            .Setup(x => x.GetUserPrincipalId())
            .Returns((int?)null);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<UnauthorizedException>()
            .WithMessage("User is not logged in!");
        
        _reviewRepository.Verify(x => x.Remove(It.IsAny<Review>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Fails_to_update_review_if_could_not_found_user_by_id()
    {
        // Arrange
        const long reviewId = 1;
        const long userId = 1;

        var command = new DeleteReviewCommand(reviewId);
        var review = new Review { Rate = 3, Content = "content", UserId = userId };

        _reviewRepository
            .Setup(x => x.GetAsync(reviewId))
            .ReturnsAsync(review);

        _userPrincipalService
            .Setup(x => x.GetUserPrincipalId())
            .Returns((int?)userId);

        _userRepository
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync((User)null!);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User not found for id = {userId}");
        
        _reviewRepository.Verify(x => x.Remove(It.IsAny<Review>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Deletes_existing_review_if_user_is_not_admin_and_author()
    {
        // Arrange
        const long reviewId = 1;
        const long userId = 2;
        const long authorId = 1;

        var command = new DeleteReviewCommand(reviewId);
        
        var user = new User { Id = userId, Username = "User", Role = Role.User };
        var review = new Review { Rate = 3, Content = "content", UserId = authorId };

        _reviewRepository
            .Setup(x => x.GetAsync(reviewId))
            .ReturnsAsync(review);

        _userPrincipalService
            .Setup(x => x.GetUserPrincipalId())
            .Returns((int?)userId);

        _userRepository
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync(user);

        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<ForbiddenException>()
            .WithMessage($"User with username {user.Username} does not have permission to delete this review");

        _reviewRepository.Verify(x => x.Remove(It.IsAny<Review>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
}