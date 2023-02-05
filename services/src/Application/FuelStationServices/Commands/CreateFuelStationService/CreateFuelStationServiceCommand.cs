using Application.Models;
using MediatR;

namespace Application.FuelStationServices.Commands.CreateFuelStationService;

public sealed record CreateFuelStationServiceCommand(string Name) 
    : IRequest<FuelStationServiceDto>;