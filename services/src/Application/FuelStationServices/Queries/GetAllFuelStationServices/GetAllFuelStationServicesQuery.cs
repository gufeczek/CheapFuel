using Application.Models;
using MediatR;

namespace Application.FuelStationServices.Queries.GetAllFuelStationServices;

public sealed record GetAllFuelStationServicesQuery 
    : IRequest<IEnumerable<FuelStationServiceDto>>;