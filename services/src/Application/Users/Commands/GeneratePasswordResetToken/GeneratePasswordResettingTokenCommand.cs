using MediatR;

namespace Application.Users.Commands.GeneratePasswordResetToken;

public sealed record GeneratePasswordResetTokenCommand(string Email) : IRequest<Unit>;
