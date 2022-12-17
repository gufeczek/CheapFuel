using Application.Models;
using Application.Models.FuelStationDtos;
using MediatR;

namespace Application.FuelStations.Commands.CreateFuelStation;

public sealed record CreateFuelStationCommand(NewFuelStationDto? FuelStationDto) 
    : IRequest<FuelStationDto>;