using MediatR;

namespace Application.FuelStationServices.Commands.DeleteService;

public sealed record DeleteFuelStationServiceCommand(long Id) 
    : IRequest<Unit>;