using MediatR;

namespace Application.Users.Queries.GetUser;

public sealed record GetUserQuery : IRequest<UserDto>
{
    public string Username { get; init; } = string.Empty;
}