using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models;
using Application.Users.Commands.AuthenticateUser;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.Users.Commands.AuthenticateUser;

public class AuthenticateUserCommandHandlerTest
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IUserPasswordHasher> _passwordHasher;
    private readonly Mock<ITokenService> _tokenService;
    private readonly AuthenticateUserCommandHandler _handler;

    public AuthenticateUserCommandHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork
            .Setup(u => u.Users)
            .Returns(_userRepository.Object);

        _passwordHasher = new Mock<IUserPasswordHasher>();
        _tokenService = new Mock<ITokenService>();

        _handler = new AuthenticateUserCommandHandler(unitOfWork.Object, _passwordHasher.Object, _tokenService.Object);
    }

    [Fact]
    public async Task Authenticates_user_with_valid_credentials()
    {
        // Arrange
        const string username = "Username";
        const string password = "Password";
        const string token = "token";
        
        var user = new User { Username = username, Role = Role.User, Password = password };

        var command = new AuthenticateUserCommand(username, password);
        
        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        _passwordHasher
            .Setup(x => x.IsPasswordCorrect(user.Password, command.Password, user))
            .Returns(true);

        _tokenService
            .Setup(x => x.GenerateJwtToken(user))
            .Returns(token);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Token.Should().Be(token);
    }
    
    [Fact]
    public async Task Authentication_should_fail_for_invalid_username()
    {
        // Arrange
        const string username = "Username";
        const string password = "Password";

        var command = new AuthenticateUserCommand(username, password);
        
        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync((User)null!);
        
        // Act
        Func<Task<JwtTokenDto>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<UnauthorizedException>()
            .WithMessage("Invalid username or password");
        
        _passwordHasher.Verify(
            x => x.IsPasswordCorrect(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<User>()), 
            Times.Never);
        _tokenService.Verify(
            x => x.GenerateJwtToken(It.IsAny<User>()), 
            Times.Never);
    }

    [Fact]
    public async Task Authentication_should_fail_for_invalid_password()
    {
        // Arrange
        const string username = "Username";
        const string password = "Password";
        const string invalidPassword = "Invalid password";

        var user = new User { Username = username, Role = Role.User, Password = password };

        var command = new AuthenticateUserCommand(username, invalidPassword);
        
        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);
        
        _passwordHasher
            .Setup(x => x.IsPasswordCorrect(user.Password, command.Password, user))
            .Returns(false);
        
        // Act
        Func<Task<JwtTokenDto>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<UnauthorizedException>()
            .WithMessage("Invalid username or password");
        
        _tokenService.Verify(
            x => x.GenerateJwtToken(It.IsAny<User>()), 
            Times.Never);
    }
}