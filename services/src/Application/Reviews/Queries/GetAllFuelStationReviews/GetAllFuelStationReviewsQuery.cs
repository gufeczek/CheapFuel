using Application.Models;
using Application.Models.Pagination;
using Domain.Common.Pagination.Response;
using MediatR;

namespace Application.Reviews.Queries.GetAllFuelStationReviews;

public sealed record GetAllFuelStationReviewsQuery(long? Id, PageRequestDto PageRequestDto) 
    : IRequest<Page<FuelStationReviewDto>>;