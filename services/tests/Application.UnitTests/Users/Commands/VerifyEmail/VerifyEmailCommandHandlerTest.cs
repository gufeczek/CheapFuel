using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Users.Commands.VerifyEmail;
using Domain.Entities;
using Domain.Entities.Tokens;
using Domain.Interfaces;
using Domain.Interfaces.Repositories.Tokens;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Application.UnitTests.Users.Commands.VerifyEmail;

public class VerifyEmailCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IEmailVerificationTokenRepository> _emailVerificationTokenRepository;
    private readonly Mock<IUserPrincipalService> _userPrinciplaService;
    private readonly VerifyEmailCommandHandler _handler;
    
    public VerifyEmailCommandHandlerTest()
    {
        _emailVerificationTokenRepository = new Mock<IEmailVerificationTokenRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork
            .Setup(x => x.EmailVerificationTokens)
            .Returns(_emailVerificationTokenRepository.Object);
        _userPrinciplaService = new Mock<IUserPrincipalService>();

        _handler = new VerifyEmailCommandHandler(_unitOfWork.Object, _userPrinciplaService.Object);
    }

    [Fact]
    public async Task Verifies_user_email_for_correct_token()
    {
        // Arrange
        const string username = "User";
        const string tokenCode = "ABCDEF";
        
        var user = new User { Id = 1, Username = username, Email = "Mail@gmail.com", EmailConfirmed = false };
        var token = new EmailVerificationToken { Token = tokenCode, Count = 0, CreatedAt = DateTime.UtcNow, User = user };

        var command = new VerifyEmailCommand(tokenCode);

        _userPrinciplaService
            .Setup(x => x.GetUserName())
            .Returns(user.Username);

        _emailVerificationTokenRepository
            .Setup(x => x.GetUserToken(user.Username))
            .ReturnsAsync(token);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().Be(Unit.Value);
        user.EmailConfirmed.Should().BeTrue();
        
        _emailVerificationTokenRepository.Verify(x => x.Remove(token), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Fails_to_verify_user_email_for_not_logged_user()
    {
        // Arrange
        const string tokenCode = "ABCDEF";

        var command = new VerifyEmailCommand(tokenCode);

        _userPrinciplaService
            .Setup(x => x.GetUserName())
            .Returns((string)null!);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<UnauthorizedException>()
            .WithMessage("User is not logged in!");
        
        _emailVerificationTokenRepository.Verify(x => x.Remove(It.IsAny<EmailVerificationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Fails_to_verify_user_email_if_user_does_not_have_any_tokens()
    {
        // Arrange
        const string tokenCode = "ABCDEF";
        const string username = "User";

        var command = new VerifyEmailCommand(tokenCode);

        _userPrinciplaService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _emailVerificationTokenRepository
            .Setup(x => x.GetUserToken(username))
            .ReturnsAsync((EmailVerificationToken)null!);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Not found email verification token for user {username}.");
        
        _emailVerificationTokenRepository.Verify(x => x.Remove(It.IsAny<EmailVerificationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Fails_to_verify_user_email_if_token_has_been_created_more_than_two_hours_ago()
    {
        // Arrange
        const string tokenCode = "ABCDEF";
        const string username = "User";

        var token = new EmailVerificationToken
        {
            Token = tokenCode, 
            Count = 0, 
            CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(121))
        };

        var command = new VerifyEmailCommand(tokenCode);

        _userPrinciplaService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _emailVerificationTokenRepository
            .Setup(x => x.GetUserToken(username))
            .ReturnsAsync(token);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Token has expired");
        
        _emailVerificationTokenRepository.Verify(x => x.Remove(It.IsAny<EmailVerificationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Fails_to_verify_user_email_if_user_provided_wrong_token_more_than_three_times()
    {
        // Arrange
        const string username = "User";
        const string tokenCode = "ABCDEF";

        var token = new EmailVerificationToken { Token = tokenCode, Count = 3, CreatedAt = DateTime.UtcNow };

        var command = new VerifyEmailCommand(tokenCode);

        _userPrinciplaService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _emailVerificationTokenRepository
            .Setup(x => x.GetUserToken(username))
            .ReturnsAsync(token);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Token has expired");
        
        _emailVerificationTokenRepository.Verify(x => x.Remove(It.IsAny<EmailVerificationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Fails_to_verify_user_email_for_invalid_token()
    {
        // Arrange
        const string username = "User";
        const string tokenCode = "ABCDEF";
        const string invalidTokenCode = "123456";
        
        var token = new EmailVerificationToken { Token = tokenCode, Count = 0, CreatedAt = DateTime.UtcNow };
        
        var command = new VerifyEmailCommand(invalidTokenCode);

        _userPrinciplaService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _emailVerificationTokenRepository
            .Setup(x => x.GetUserToken(username))
            .ReturnsAsync(token);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Invalid token. Number of attempts remaining 2");

        token.Count.Should().Be(1);
        
        _emailVerificationTokenRepository.Verify(x => x.Remove(It.IsAny<EmailVerificationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }
}