using MediatR;

namespace Application.ServiceAtStations.Commands.RemoveServiceFromStation;

public sealed record RemoveServiceFromStationCommand(long FuelStationId, long ServiceId) 
    : IRequest<Unit>;