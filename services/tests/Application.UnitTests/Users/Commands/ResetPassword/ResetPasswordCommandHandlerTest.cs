using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Users.Commands.ResetPassword;
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

namespace Application.UnitTests.Users.Commands.ResetPassword;

public class ResetPasswordCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IPasswordResetTokenRepository> _passwordResetTokenRepository;
    private readonly Mock<IUserPasswordHasher> _passwordHasher;
    private readonly ResetPasswordCommandHandler _handler;
    
    public ResetPasswordCommandHandlerTest()
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

        _passwordHasher = new Mock<IUserPasswordHasher>();

        _handler = new ResetPasswordCommandHandler(
            _unitOfWork.Object,
            _passwordResetTokenRepository.Object,
            _passwordHasher.Object);
    }

    [Fact]
    public async Task Resets_user_password_for_correct_data()
    {
        // Arrange
        const string email = "email@mail.com";
        const string tokenCode = "ABCDEF";
        const string newPassword = "Password123";
        const string hashedNewPassword = "HashedPassword123";
        
        var user = new User { Username = "Username", Email = email, Role = Role.User, Password = "OldPassword" };
        var token = new PasswordResetToken { Token = tokenCode, Count = 0, User = user, CreatedAt = DateTime.UtcNow };
        
        var command = new ResetPasswordCommand(email, tokenCode, newPassword, newPassword);

        _userRepository
            .Setup(x => x.GetByEmailAddressAsync(command.Email))
            .ReturnsAsync(user);

        _passwordResetTokenRepository
            .Setup(x => x.GetUserToken(user.Username))
            .ReturnsAsync(token);

        _passwordHasher
            .Setup(x => x.HashPassword(command.NewPassword, user))
            .Returns(hashedNewPassword);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().Be(Unit.Value);
        user.Password.Should().Be(hashedNewPassword);
        
        _passwordHasher.Verify(x => x.HashPassword(command.NewPassword, It.IsAny<User>()), Times.Once);
        _passwordResetTokenRepository.Verify(x => x.Remove(token), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Reset_password_fails_for_invalid_email_address()
    {
        // Arrange
        const string email = "email@mail.com";

        var command = new ResetPasswordCommand(email, "ABCDEF", "Password123", "Password123");

        _userRepository
            .Setup(x => x.GetByEmailAddressAsync(email))
            .ReturnsAsync((User)null!);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Invalid token");
        
        _passwordHasher.Verify(x => x.HashPassword(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
        _passwordResetTokenRepository.Verify(x => x.Remove(It.IsAny<PasswordResetToken>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Reset_password_fails_if_user_does_not_have_any_tokens()
    {
        // Arrange
        const string email = "email@mail.com";
        const string tokenCode = "ABCDEF";
        
        var user = new User { Username = "Username", Email = email, Role = Role.User, Password = "OldPassword" };
        
        var command = new ResetPasswordCommand(email, tokenCode, "Password123", "Password123");

        _userRepository
            .Setup(x => x.GetByEmailAddressAsync(command.Email))
            .ReturnsAsync(user);

        _passwordResetTokenRepository
            .Setup(x => x.GetUserToken(user.Username))
            .ReturnsAsync((PasswordResetToken)null!);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Invalid token");
        
        _passwordHasher.Verify(x => x.HashPassword(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
        _passwordResetTokenRepository.Verify(x => x.Remove(It.IsAny<PasswordResetToken>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Reset_password_fails_if_token_has_been_created_more_than_thirty_minutes_ago()
    {
        // Arrange
        const string email = "email@mail.com";
        const string tokenCode = "ABCDEF";
        
        var user = new User { Username = "Username", Email = email, Role = Role.User, Password = "OldPassword" };
        var token = new PasswordResetToken { Token = tokenCode, Count = 0, User = user, CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(31)) };

        var command = new ResetPasswordCommand(email, tokenCode, "Password123", "Password123");
        
        _userRepository
            .Setup(x => x.GetByEmailAddressAsync(command.Email))
            .ReturnsAsync(user);

        _passwordResetTokenRepository
            .Setup(x => x.GetUserToken(user.Username))
            .ReturnsAsync(token);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Token has expired");
        
        _passwordHasher.Verify(x => x.HashPassword(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
        _passwordResetTokenRepository.Verify(x => x.Remove(It.IsAny<PasswordResetToken>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Reset_password_fails_if_user_provided_wrong_token_more_than_three_times()
    {
        // Arrange
        const string email = "email@mail.com";
        const string tokenCode = "ABCDEF";
        
        var user = new User { Username = "Username", Email = email, Role = Role.User, Password = "OldPassword" };
        var token = new PasswordResetToken { Token = tokenCode, Count = 3, User = user, CreatedAt = DateTime.UtcNow };

        var command = new ResetPasswordCommand(email, tokenCode, "Password123", "Password123");
        
        _userRepository
            .Setup(x => x.GetByEmailAddressAsync(command.Email))
            .ReturnsAsync(user);

        _passwordResetTokenRepository
            .Setup(x => x.GetUserToken(user.Username))
            .ReturnsAsync(token);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Token has expired");
        
        _passwordHasher.Verify(x => x.HashPassword(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
        _passwordResetTokenRepository.Verify(x => x.Remove(It.IsAny<PasswordResetToken>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Reset_password_fails_for_invalid_token()
    {
        // Arrange
        const string email = "email@mail.com";
        const string invalidTokenCode = "123456";
        const string tokenCode = "ABCDEF";
        
        var user = new User { Username = "Username", Email = email, Role = Role.User, Password = "OldPassword" };
        var token = new PasswordResetToken { Token = tokenCode, Count = 0, User = user, CreatedAt = DateTime.UtcNow };

        var command = new ResetPasswordCommand(email, invalidTokenCode, "Password123", "Password123");
        
        _userRepository
            .Setup(x => x.GetByEmailAddressAsync(command.Email))
            .ReturnsAsync(user);

        _passwordResetTokenRepository
            .Setup(x => x.GetUserToken(user.Username))
            .ReturnsAsync(token);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Invalid token. Number of attempts remaining 2");

        token.Count.Should().Be(1);
        
        _passwordHasher.Verify(x => x.HashPassword(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
        _passwordResetTokenRepository.Verify(x => x.Remove(It.IsAny<PasswordResetToken>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }
}