using Application.Models;
using MediatR;

namespace Application.FuelStations.Queries.GetAllFuelStationForMap;

public sealed record GetAllFuelStationsForMapQuery(FuelStationFilterDto? FilterDto) 
    : IRequest<IEnumerable<SimpleMapFuelStationDto>>;