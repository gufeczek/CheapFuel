using Application.Models;
using Application.Models.Pagination;
using Domain.Common.Pagination.Response;
using MediatR;

namespace Application.FuelStationServices.Queries.GetAllFuelStationServices;

public sealed record GetAllFuelStationServicesQuery(PageRequestDto PageRequestDto) 
    : IRequest<Page<FuelStationServiceDto>>;