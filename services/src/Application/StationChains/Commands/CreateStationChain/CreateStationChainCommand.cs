using Domain.Entities;
using MediatR;

namespace Application.StationChains.Commands.CreateStationChain;

public sealed record CreateStationChainCommand : IRequest<StationChain>
{
    public string Name { get; init; } = string.Empty;
}