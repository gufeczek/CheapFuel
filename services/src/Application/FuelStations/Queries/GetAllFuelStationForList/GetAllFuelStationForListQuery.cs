using Application.Models;
using Application.Models.Filters;
using Application.Models.Pagination;
using Domain.Common.Pagination.Response;
using MediatR;

namespace Application.FuelStations.Queries.GetAllFuelStationForList;

public sealed record GetAllFuelStationForListQuery(
    FuelStationFilterWithLocalizationDto? Filter, 
    PageRequestDto PageRequest) : IRequest<Page<SimpleFuelStationDto>>;