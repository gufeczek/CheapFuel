using Application.Models;
using MediatR;

namespace Application.StationChains.Commands.CreateStationChain;

public sealed record CreateStationChainCommand(string Name) 
    : IRequest<StationChainDto>;