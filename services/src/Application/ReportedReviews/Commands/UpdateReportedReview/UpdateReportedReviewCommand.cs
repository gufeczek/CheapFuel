using Application.Models;
using MediatR;

namespace Application.ReportedReviews.Commands.UpdateReportedReview;

public sealed record UpdateReportedReviewCommand(
    long? ReviewId,
    long? UserId,
    UpdateReportedReviewDto? ReportedReviewDto) : IRequest<UpdateReportedReviewDto>;