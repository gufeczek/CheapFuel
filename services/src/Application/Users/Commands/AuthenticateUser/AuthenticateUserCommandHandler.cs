using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Users.Commands.AuthenticateUser;

public sealed class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, JwtTokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthenticateUserCommandHandler(IUnitOfWork unitOfWork, IUserPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _userRepository = unitOfWork.Users;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }
    
    public async Task<JwtTokenDto> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null)
        {
            throw new UnauthorizedException("Invalid username or password");
        }

        var isPasswordCorrect = _passwordHasher.IsPasswordCorrect(user.Password, request.Password, user);

        if (!isPasswordCorrect)
        {
            throw new UnauthorizedException("Invalid username or password");
        }

        var token = _tokenService.GenerateJwtToken(user);
        
        return new JwtTokenDto(token);
    }
}