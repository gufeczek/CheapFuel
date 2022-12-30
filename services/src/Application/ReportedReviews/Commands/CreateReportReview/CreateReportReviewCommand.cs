using Application.Models;
using MediatR;

namespace Application.ReportedReviews.Commands.CreateReportReview;

public sealed record CreateReportReviewCommand(long? ReviewId) : IRequest<CreateFuelStationReportedReviewDto>;
