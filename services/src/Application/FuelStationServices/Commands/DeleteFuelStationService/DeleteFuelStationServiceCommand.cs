using MediatR;

namespace Application.FuelStationServices.Commands.DeleteFuelStationService;

public sealed record DeleteFuelStationServiceCommand(long? Id) 
    : IRequest<Unit>;