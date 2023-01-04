using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models;
using Application.Users.Commands.RegisterUser;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Tokens;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Tokens;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.Users.Commands.RegisterUser;

public class RegisterUserCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IEmailVerificationTokenRepository> _emailVerificationTokenRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IUserPasswordHasher> _passwordHasher;
    private readonly RegisterUserCommandHandler _handler;
    
    public RegisterUserCommandHandlerTest()
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

        _passwordHasher = new Mock<IUserPasswordHasher>();
        _mapper = new Mock<IMapper>();

        _handler = new RegisterUserCommandHandler(
            _unitOfWork.Object, 
            _mapper.Object, 
            _passwordHasher.Object, 
            new Mock<ITokenService>().Object, 
            new Mock<IEmailSenderService>().Object);
    }

    [Fact]
    public async Task Registers_user_when_all_data_is_correct()
    {
        // Arrange
        const string username = "Username";
        const string email = "Email@gmail.com";
        const string password = "Password";
        const string passwordHash = "Password hash";

        var userDetailsDto = new UserDetailsDto() { Username = username, Email = email, Role = Role.User };
        
        var command = new RegisterUserCommand(username, email, password, password);

        _userRepository
            .Setup(x => x.ExistsByUsername(username))
            .ReturnsAsync(false);

        _userRepository
            .Setup(x => x.ExistsByEmail(email))
            .ReturnsAsync(false);

        _passwordHasher
            .Setup(x => x.HashPassword(command.Password, It.IsAny<User>()))
            .Returns(passwordHash);

        _mapper
            .Setup(x => x.Map<UserDetailsDto>(It.IsAny<User>()))
            .Returns(userDetailsDto);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        
        _userRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
        _emailVerificationTokenRepository.Verify(x => x.Add(It.IsAny<EmailVerificationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Registration_fails_for_duplicate_username()
    {
        // Arrange
        const string username = "Username";
        const string email = "Email@gmail.com";
        const string password = "Password";

        var command = new RegisterUserCommand(username, email, password, password);

        _userRepository
            .Setup(x => x.ExistsByUsername(command.Username))
            .ReturnsAsync(true);
        
        // Act
        Func<Task<UserDetailsDto>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<DuplicateCredentialsException>()
            .WithMessage("Username is already taken");
        
        _userRepository.Verify(x => x.ExistsByEmail(It.IsAny<string>()), Times.Never);
        _passwordHasher.Verify(x => x.HashPassword(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
        _userRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Registration_fails_for_duplicate_email()
    {
        // Arrange
        const string username = "Username";
        const string email = "Email@gmail.com";
        const string password = "Password";

        var command = new RegisterUserCommand(username, email, password, password);

        _userRepository
            .Setup(x => x.ExistsByUsername(command.Username))
            .ReturnsAsync(false);

        _userRepository
            .Setup(x => x.ExistsByEmail(command.Email))
            .ReturnsAsync(true);
        
        // Act
        Func<Task<UserDetailsDto>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<DuplicateCredentialsException>()
            .WithMessage("Email is already taken");
        
        _passwordHasher.Verify(x => x.HashPassword(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
        _userRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
}