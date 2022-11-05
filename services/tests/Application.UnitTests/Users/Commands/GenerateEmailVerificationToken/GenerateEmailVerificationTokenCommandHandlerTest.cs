using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Users.Commands.GenerateEmailVerificationToken;
using Domain.Entities;
using Domain.Entities.Tokens;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Tokens;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Application.UnitTests.Users.Commands.GenerateEmailVerificationToken;

public class GenerateEmailVerificationTokenCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IEmailVerificationTokenRepository> _emailVerificationTokenRepository;
    private readonly Mock<IUserPrincipalService> _userPrincipalService;
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<IEmailSenderService> _emailSenderService;
    private readonly GenerateEmailVerificationTokenCommandHandler _handler;
    
    public GenerateEmailVerificationTokenCommandHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _emailVerificationTokenRepository = new Mock<IEmailVerificationTokenRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork
            .Setup(u => u.Users)
            .Returns(_userRepository.Object);
        _unitOfWork
            .Setup(u => u.EmailVerificationTokens)
            .Returns(_emailVerificationTokenRepository.Object);

        _userPrincipalService = new Mock<IUserPrincipalService>();
        _tokenService = new Mock<ITokenService>();
        _emailSenderService = new Mock<IEmailSenderService>();

        _handler = new GenerateEmailVerificationTokenCommandHandler(
            _unitOfWork.Object,
            _userPrincipalService.Object,
            _tokenService.Object,
            _emailSenderService.Object);
    }

    [Fact]
    public async Task Generates_email_verification_token()
    {
        // Arrange
        const string username = "User";
        const string email = "email@gmail.com";
        const string tokenCode = "ABCDEF";

        var user = new User { Username = username, Email = email, EmailConfirmed = false };

        var command = new GenerateEmailVerificationTokenCommand();
        
        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        _tokenService
            .Setup(x => x.GenerateSimpleToken())
            .Returns(tokenCode);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().Be(Unit.Value);
        
        _emailVerificationTokenRepository.Verify(x => x.RemoveAllByUsername(username), Times.Once);
        _emailVerificationTokenRepository.Verify(x => x.Add(It.IsAny<EmailVerificationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        _emailSenderService.Verify(x => x.SendEmailAddressVerificationToken(user.Email, tokenCode));
    }

    [Fact]
    public async Task Fails_to_generate_token_if_user_not_logged_in()
    {
        // Arrange
        var command = new GenerateEmailVerificationTokenCommand();

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
        
        _emailVerificationTokenRepository.Verify(x => x.RemoveAllByUsername(It.IsAny<string>()), Times.Never);
        _emailVerificationTokenRepository.Verify(x => x.Add(It.IsAny<EmailVerificationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
        _emailSenderService.Verify(x => x.SendEmailAddressVerificationToken(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Fails_to_generate_token_if_user_not_found()
    {
        // Arrange
        const string username = "User";
        
        var command = new GenerateEmailVerificationTokenCommand();

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync((User)null!);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>();
        
        _emailVerificationTokenRepository.Verify(x => x.RemoveAllByUsername(It.IsAny<string>()), Times.Never);
        _emailVerificationTokenRepository.Verify(x => x.Add(It.IsAny<EmailVerificationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
        _emailSenderService.Verify(x => x.SendEmailAddressVerificationToken(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Fails_to_generate_token_if_user_has_already_verified_email_address()
    {
        // Arrange
        const string username = "User";
        const string email = "email@gmail.com";

        var user = new User { Username = username, Email = email, EmailConfirmed = true };

        var command = new GenerateEmailVerificationTokenCommand();
        
        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Email is already verified");
        
        _emailVerificationTokenRepository.Verify(x => x.RemoveAllByUsername(It.IsAny<string>()), Times.Never);
        _emailVerificationTokenRepository.Verify(x => x.Add(It.IsAny<EmailVerificationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
        _emailSenderService.Verify(x => x.SendEmailAddressVerificationToken(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}