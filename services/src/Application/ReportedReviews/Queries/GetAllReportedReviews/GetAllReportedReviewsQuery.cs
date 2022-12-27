using Application.Models;
using Application.Models.Pagination;
using Domain.Common.Pagination.Response;
using MediatR;

namespace Application.ReportedReviews.Queries.GetAllReportedReviews;

public sealed record GetAllReportedReviewsQuery(PageRequestDto PageRequestDto) : IRequest<Page<FuelStationReportedReviewDto>>;