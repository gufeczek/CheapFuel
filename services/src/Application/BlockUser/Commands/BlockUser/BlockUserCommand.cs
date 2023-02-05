using Application.Models;
using MediatR;

namespace Application.BlockUser.Commands.BlockUser;

public sealed record BlockUserCommand(
    string? Username,
    string Reason) : IRequest<BlockUserDto>;
