using Application.Models;
using Application.Models.Pagination;
using Domain.Common.Pagination.Response;
using MediatR;

namespace Application.StationChains.Queries.GetAllStationChainsQuery;

public sealed record GetAllStationChainsQuery(PageRequestDto PageRequestDto)
    : IRequest<Page<StationChainDto>>;
