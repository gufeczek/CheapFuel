using Application.Models;
using MediatR;

namespace Application.BlockUser.Commands.UnblockUser;

public sealed record UnblockUserCommand(long? UserId) : IRequest<Unit>;