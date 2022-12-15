using MediatR;

namespace Application.FuelAtStations.Commands.RemoveFuelFromStation;

public sealed record RemoveFuelFromStationCommand(long FuelStationId, long FuelTypeId) 
    : IRequest<Unit>;