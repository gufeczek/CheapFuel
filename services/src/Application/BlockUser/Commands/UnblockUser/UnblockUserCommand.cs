using Application.Models;
using MediatR;

namespace Application.BlockUser.Commands.UnblockUser;

public sealed record UnblockUserCommand(string? Username) : IRequest<Unit>;