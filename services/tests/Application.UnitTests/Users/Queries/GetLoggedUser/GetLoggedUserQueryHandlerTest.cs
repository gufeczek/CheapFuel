using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models;
using Application.Users.Queries.GetLoggedUser;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.Users.Queries.GetLoggedUser;

public class GetLoggedUserQueryHandlerTest
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IUserPrincipalService> _userPrincipalService;
    private readonly GetLoggedUserQueryHandler _handler;

    public GetLoggedUserQueryHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _userPrincipalService = new Mock<IUserPrincipalService>();
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork
            .Setup(u => u.Users)
            .Returns(_userRepository.Object);
        _mapper = new Mock<IMapper>();

        _handler = new GetLoggedUserQueryHandler(unitOfWork.Object, _mapper.Object, _userPrincipalService.Object);
    }

    [Fact]
    public async Task Return_logged_user_info()
    {
        // Arrange
        var user = new User { Id = 2,Username = "Krzyś", Role = Role.User };
        var userDto = new UserDetailsDto() {Username = "Krzyś", Role = Role.User };
        var query = new GetLoggedUserQuery();

        _userPrincipalService
            .Setup(x => x.GetUserPrincipalId())
            .Returns((int?)user.Id);
        
        _userRepository
            .Setup(x => x.GetAsync(user.Id))
            .ReturnsAsync(user);

        _mapper
            .Setup(x => x.Map<UserDetailsDto>(user))
            .Returns(userDto);
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert 
        result.Should().Be(userDto);
    }
}