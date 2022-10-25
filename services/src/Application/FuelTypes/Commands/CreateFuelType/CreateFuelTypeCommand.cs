using Application.Models;
using MediatR;

namespace Application.FuelTypes.Commands.CreateFuelType;

public sealed record CreateFuelTypeCommand(string Name) 
    : IRequest<FuelTypeDto>;