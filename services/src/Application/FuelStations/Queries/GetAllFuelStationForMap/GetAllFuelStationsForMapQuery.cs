using Application.Models;
using Application.Models.Filters;
using MediatR;

namespace Application.FuelStations.Queries.GetAllFuelStationForMap;

public sealed record GetAllFuelStationsForMapQuery(FuelStationFilterDto? FilterDto) 
    : IRequest<IEnumerable<SimpleFuelStationDto>>;