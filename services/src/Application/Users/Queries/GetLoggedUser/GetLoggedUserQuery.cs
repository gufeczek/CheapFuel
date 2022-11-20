using Application.Models;
using MediatR;

namespace Application.Users.Queries.GetLoggedUser;

public sealed record GetLoggedUserQuery() : IRequest<UserDetailsDto>
{
}