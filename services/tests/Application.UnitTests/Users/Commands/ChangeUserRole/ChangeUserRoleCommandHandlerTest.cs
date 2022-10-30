using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Users.Commands.ChangeUserRole;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Application.UnitTests.Users.Commands.ChangeUserRole;

public class ChangeUserRoleCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly ChangeUserRoleCommandHandler _handler;

    public ChangeUserRoleCommandHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork
            .Setup(u => u.Users)
            .Returns(_userRepository.Object);

        _handler = new ChangeUserRoleCommandHandler(_unitOfWork.Object);
    }

    [Fact]
    public async Task Changes_role_of_user()
    {
        // Arrange
        const string username = "Username";
        const Role role = Role.Admin;

        var user = new User { Username = username, Role = Role.User };

        var command = new ChangeUserRoleCommand(username, role);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        user.Role.Should().Be(Role.Admin);
        
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Throws_exception_for_wrong_username()
    {
        // Arrange
        const string username = "Username";
        const Role role = Role.Admin;

        var command = new ChangeUserRoleCommand(username, role);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync((User)null!);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User not found for username = {username}");
        
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Throws_exception_when_role_is_equal_to_null()
    {
        // Arrange
        const string username = "Username";

        var user = new User { Username = username, Role = Role.User };

        var command = new ChangeUserRoleCommand(username, null);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);
        
        // Act
        Func<Task<Unit>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<ArgumentNullException>();
        
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
}