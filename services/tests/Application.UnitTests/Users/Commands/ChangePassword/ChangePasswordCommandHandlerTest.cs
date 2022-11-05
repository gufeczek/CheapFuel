using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Users.Commands.ChangePassword;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Application.UnitTests.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IUserPasswordHasher> _passwordHasher;
    private readonly Mock<IUserPrincipalService> _userPrincipalService;
    private readonly ChangePasswordCommandHandler _handler;
    
    public ChangePasswordCommandHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork
            .Setup(u => u.Users)
            .Returns(_userRepository.Object);

        _passwordHasher = new Mock<IUserPasswordHasher>();
        _userPrincipalService = new Mock<IUserPrincipalService>();
        _handler = new ChangePasswordCommandHandler(
            _unitOfWork.Object, 
            _passwordHasher.Object, 
            _userPrincipalService.Object);
    }

    [Fact]
    public async Task Changes_user_password()
    {
        // Arrange
        const string username = "Username";
        const string oldPassword = "oldPassword";
        const string newPassword = "newPassword";
        const string hashedNewPassword = "Ffdsjk2432fds";
        
        var user = new User { Username = username, Role = Role.User, Password = oldPassword };

        var command = new ChangePasswordCommand(oldPassword, newPassword, newPassword);

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x => x.IsPasswordCorrect(user.Password, command.OldPassword, user))
            .Returns(true);

        _passwordHasher
            .Setup(x => x.IsPasswordCorrect(user.Password, command.NewPassword, user))
            .Returns(false);

        _passwordHasher
            .Setup(x => x.HashPassword(command.NewPassword, user))
            .Returns(hashedNewPassword);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().Be(Unit.Value);
        user.Password.Should().Be(hashedNewPassword);
        
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Password_changing_fails_if_user_not_logged()
    {
        // Arrange
        var command = new ChangePasswordCommand("Password123", "Password321", "Password321");
        
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
        
        _userRepository.Verify(x => x.GetByUsernameAsync(It.IsAny<string>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Password_changing_fails_if_user_not_exists()
    {
        // Arrange
        const string username = "Username";
        
        var command = new ChangePasswordCommand("Password123", "Password321", "Password321");

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
        
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Password_changing_fails_if_old_password_is_incorrect()
    {
        // Arrange
        const string username = "Username";
        const string oldPassword = "Password123";
        const string incorrectPassword = "IncorrectPassword";

        var user = new User { Username = username, Role = Role.User, Password = oldPassword };

        var command = new ChangePasswordCommand(incorrectPassword, "Password321", "Password321");
        
        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x => x.IsPasswordCorrect(user.Password, command.OldPassword, user))
            .Returns(false);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<UnauthorizedException>()
            .WithMessage("Invalid password");
        
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Password_changing_fails_if_new_password_is_same_as_old()
    {
        // Arrange
        const string username = "Username";
        const string oldPassword = "Password123";

        var user = new User { Username = username, Role = Role.User, Password = oldPassword };

        var command = new ChangePasswordCommand(oldPassword, oldPassword, oldPassword);
        
        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x => x.IsPasswordCorrect(user.Password, command.OldPassword, user))
            .Returns(true);

        _passwordHasher
            .Setup(x => x.IsPasswordCorrect(user.Password, command.NewPassword, user))
            .Returns(true);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("The new password cannot be the same as the old password");
        
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
}