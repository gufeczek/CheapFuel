using MediatR;

namespace Application.Users.Commands.AuthenticateUser;

public sealed record AuthenticateUserCommand : IRequest<string>
{
    public string Username { get; init; }
    public string Password { get; init; }
}