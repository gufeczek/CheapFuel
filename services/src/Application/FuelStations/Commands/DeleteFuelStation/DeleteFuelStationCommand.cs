using MediatR;

namespace Application.FuelStations.Commands.DeleteFuelStation;

public sealed record DeleteFuelStationCommand(long? FuelStationId) 
    : IRequest<Unit>;