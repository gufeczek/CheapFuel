using Application.Models;
using MediatR;

namespace Application.FuelTypes.Queries.GetAllFuelTypes;

public sealed record GetAllFuelTypesQuery : IRequest<IEnumerable<FuelTypeDto>>;