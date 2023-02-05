using Application.Models.OwnedStations;
using MediatR;

namespace Application.OwnerStations.Commands.CreateFuelStationOwner;

public sealed record CreateFuelStationOwnerCommand(long? FuelStationId, long? UserId) 
    : IRequest<OwnedStationDto>;