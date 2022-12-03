using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Users.Commands.DeactivateUser;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Application.UnitTests.Users.Commands.DeactivateUser;

public class DeactivateUserCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IUserPrincipalService> _userPrincipalService;
    private readonly DeactivateUserCommandHandler _handler;
    
    public DeactivateUserCommandHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork
            .Setup(u => u.Users)
            .Returns(_userRepository.Object);
        _userPrincipalService = new Mock<IUserPrincipalService>();

        _handler = new DeactivateUserCommandHandler(_unitOfWork.Object, _userPrincipalService.Object);
    }

    [Fact]
    public async Task Deactivate_user_account_if_performed_by_logged_user()
    {
        // Arrange
        const string username = "User";
        var user = new User { Id = 1, Username = username };

        var command = new DeactivateUserCommand(username);
        
        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().Be(Unit.Value);
        
        _userRepository.Verify(x => x.Remove(user), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Deactivate_user_account_if_performed_by_admin()
    {
        // Arrange
        const string userUsername = "User";
        const string adminUsername = "Admin";
        var adminUser = new User { Id = 1, Username = adminUsername, Role = Role.Admin };
        var userToRemove = new User { Id = 2, Username = userUsername, Role = Role.User };
        
        var command = new DeactivateUserCommand(userUsername);

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(adminUsername);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(adminUsername))
            .ReturnsAsync(adminUser);
        
        _userRepository
            .Setup(x => x.GetByUsernameAsync(userUsername))
            .ReturnsAsync(userToRemove);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().Be(Unit.Value);
        
        _userRepository.Verify(x => x.Remove(userToRemove));
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Deactivate_user_account_fails_if_performed_not_by_admin()
    {
        // Arrange
        const string loggedUserUsername = "LoggedUser";
        const string userToRemoveUsername = "User";
        var loggedUser = new User { Id = 1, Username = loggedUserUsername, Role = Role.User };
        var userToRemove = new User { Id = 2, Username = userToRemoveUsername, Role = Role.User };
        
        var command = new DeactivateUserCommand(userToRemoveUsername);

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(loggedUserUsername);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(loggedUserUsername))
            .ReturnsAsync(loggedUser);
        
        _userRepository
            .Setup(x => x.GetByUsernameAsync(userToRemoveUsername))
            .ReturnsAsync(userToRemove);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<ForbiddenException>();
        
        _userRepository.Verify(x => x.Remove(It.IsAny<User>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
}