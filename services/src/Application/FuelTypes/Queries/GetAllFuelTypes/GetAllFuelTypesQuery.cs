using Application.Models;
using Application.Models.Pagination;
using Domain.Common.Pagination.Response;
using MediatR;

namespace Application.FuelTypes.Queries.GetAllFuelTypes;

public sealed record GetAllFuelTypesQuery(PageRequestDto PageRequestDto) 
    : IRequest<Page<FuelTypeDto>>;