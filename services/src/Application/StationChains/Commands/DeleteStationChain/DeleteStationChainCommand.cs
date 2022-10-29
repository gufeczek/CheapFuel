using MediatR;

namespace Application.StationChains.Commands.DeleteStationChain;

public sealed record DeleteStationChainCommand(long? Id)
    : IRequest<Unit>;