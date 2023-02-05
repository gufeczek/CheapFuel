using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authentication;
using Application.Common.Interfaces;
using Application.Users.Commands.GeneratePasswordResetToken;
using Domain.Entities;
using Domain.Entities.Tokens;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Tokens;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Application.UnitTests.Users.Commands.GeneratePasswordResetToken;

public class GeneratePasswordResetTokenCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IPasswordResetTokenRepository> _passwordResetTokenRepository;
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<IEmailSenderService> _emailSenderService;
    private readonly GeneratePasswordResetTokenCommandHandler _handler;

    public GeneratePasswordResetTokenCommandHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _passwordResetTokenRepository = new Mock<IPasswordResetTokenRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork
            .Setup(u => u.Users)
            .Returns(_userRepository.Object);
        _unitOfWork
            .Setup(p => p.PasswordResetTokenRepository)
            .Returns(_passwordResetTokenRepository.Object);
        
        _tokenService = new Mock<ITokenService>();
        _emailSenderService = new Mock<IEmailSenderService>();

        _handler = new GeneratePasswordResetTokenCommandHandler(
            _unitOfWork.Object, 
            _tokenService.Object,
            _emailSenderService.Object);
    }

    [Fact]
    public async Task Generates_password_reset_token_for_correct_email()
    {
        // Arrange
        const string email = "email@gmail.com";
        const string tokenCode = "ABCDEF";
        
        var user = new User { Username = "Username", Role = Role.User, Email = email, Password = "Password123" };

        var command = new GeneratePasswordResetTokenCommand(email);

        _userRepository
            .Setup(x => x.GetByEmailAddressAsync(command.Email))
            .ReturnsAsync(user);

        _tokenService
            .Setup(x => x.GenerateSimpleToken())
            .Returns(tokenCode);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        
        _passwordResetTokenRepository.Verify(x => x.RemoveAllByUsername(user.Username), Times.Once);
        _passwordResetTokenRepository.Verify(x => x.Add(It.IsAny<PasswordResetToken>()), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        _emailSenderService.Verify(x => x.SendPasswordResetToken(user.Email, tokenCode));
    }

    [Fact]
    public async Task Fails_silently_if_user_with_given_email_was_not_found()
    {
        // Arrange
        const string email = "email@gmail.com";

        var command = new GeneratePasswordResetTokenCommand(email);

        _userRepository
            .Setup(x => x.GetByEmailAddressAsync(command.Email))
            .ReturnsAsync((User)null!);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().Be(Unit.Value);
        
        _passwordResetTokenRepository.Verify(x => x.RemoveAllByUsername(It.IsAny<string>()), Times.Never);
        _passwordResetTokenRepository.Verify(x => x.Add(It.IsAny<PasswordResetToken>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
        _emailSenderService.Verify(x => x.SendPasswordResetToken(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}