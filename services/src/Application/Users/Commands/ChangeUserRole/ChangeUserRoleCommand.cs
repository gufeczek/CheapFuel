using Domain.Enums;
using MediatR;

namespace Application.Users.Commands.ChangeUserRole;

public sealed record ChangeUserRoleCommand(string Username, Role? Role) 
    : IRequest<Unit>;