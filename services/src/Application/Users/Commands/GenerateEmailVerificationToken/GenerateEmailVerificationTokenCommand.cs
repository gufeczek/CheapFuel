using MediatR;

namespace Application.Users.Commands.GenerateEmailVerificationToken;

public sealed record GenerateEmailVerificationTokenCommand : IRequest;