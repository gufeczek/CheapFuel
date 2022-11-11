using Application.Models;
using MediatR;

namespace Application.Users.Commands.AuthenticateUser;

public sealed record AuthenticateUserCommand(string Username, string Password) 
    : IRequest<JwtTokenDto>;