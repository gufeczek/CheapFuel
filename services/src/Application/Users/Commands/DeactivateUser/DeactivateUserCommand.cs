using MediatR;

namespace Application.Users.Commands.DeactivateUser;

public sealed record DeactivateUserCommand(string? Username) 
    : IRequest<Unit>;