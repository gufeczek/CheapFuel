using Application.Models;
using MediatR;

namespace Application.Users.Queries.GetUserForAdministration;

public sealed record GetUserForAdministrationQuery(string Username) 
    : IRequest<UserDetailsDto>;