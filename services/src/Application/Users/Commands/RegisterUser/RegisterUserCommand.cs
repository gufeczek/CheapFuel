using Application.Models;
using MediatR;

namespace Application.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand : IRequest<UserDto>
{
    public string Username { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
}