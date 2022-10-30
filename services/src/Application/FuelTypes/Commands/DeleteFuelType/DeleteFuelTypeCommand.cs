using MediatR;

namespace Application.FuelTypes.Commands.DeleteFuelType;

public sealed record DeleteFuelTypeCommand(long? Id) 
    : IRequest<Unit>;