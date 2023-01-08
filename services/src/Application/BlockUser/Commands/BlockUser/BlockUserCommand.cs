using Application.Models;
using MediatR;

namespace Application.BlockUser.Commands.BlockUser;

public sealed record BlockUserCommand(
    long? UserId,
    string Reason) : IRequest<BlockUserDto>;
