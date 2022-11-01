using MediatR;

namespace Application.Users.Commands.VerifyEmail;

public sealed record VerifyEmailCommand(string Token) : IRequest<Unit>;