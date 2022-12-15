using Application.Models;
using MediatR;

namespace Application.ServiceAtStations.Commands.AddServiceToStation;

public sealed record AddServiceToStationCommand(long FuelStationId, long ServiceId) 
    : IRequest<ServiceAtStationDto>;
