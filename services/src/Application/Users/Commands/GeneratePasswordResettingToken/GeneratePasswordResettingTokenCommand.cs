using MediatR;

namespace Application.Users.Commands.GeneratePasswordResettingToken;

public sealed record GeneratePasswordResettingTokenCommand(string Email) : IRequest<Unit>;
