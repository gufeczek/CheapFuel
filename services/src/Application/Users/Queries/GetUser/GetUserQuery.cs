using Application.Models;
using MediatR;

namespace Application.Users.Queries.GetUser;

public sealed record GetUserQuery(string Username) : IRequest<UserDto>;