using Application.Models;
using MediatR;

namespace Application.FuelAtStations.Commands.AddFuelToStation;

public sealed record AddFuelToStationCommand(long FuelStationId, long FuelTypeId) 
    : IRequest<FuelAtStationDto>;