using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Models;
using Application.Users.Queries.GetUser;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.Users.Queries.GetUser;

public class GetUserQueryHandlerTest
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly GetUserQueryHandler _handler;

    public GetUserQueryHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork
            .Setup(u => u.Users)
            .Returns(_userRepository.Object);
        _mapper = new Mock<IMapper>();

        _handler = new GetUserQueryHandler(unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task Returns_user_if_given_correct_username()
    {
        // Arrange
        const string username = "Username";

        var user = new User { Username = username, Role = Role.User };
        var userDto = new UserDto { Username = username, Role = Role.User };

        var query = new GetUserQuery(username);
        
        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        _mapper
            .Setup(x => x.Map<UserDto>(user))
            .Returns(userDto);
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.Should().Be(userDto);
    }

    [Fact]
    public async Task Throws_exception_for_wrong_username()
    {
        // Arrange
        const string username = "Username";

        var query = new GetUserQuery(username);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync((User)null!);
        
        // Act
        Func<Task<UserDto>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));

        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"{nameof(User)} not found for {nameof(User.Username)} = {username}");
        
        _mapper.Verify(x => x.Map<UserDto>(It.IsAny<User>()), Times.Never);
    }
}