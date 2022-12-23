using Application.Models;
using Application.Models.Pagination;
using Domain.Common.Pagination.Response;
using MediatR;

namespace Application.Reviews.Queries.GetAllUserReviews;

public sealed record GetAllUserReviewsQuery(string? Username, PageRequestDto PageRequest) 
    : IRequest<Page<FuelStationReviewDto>>;