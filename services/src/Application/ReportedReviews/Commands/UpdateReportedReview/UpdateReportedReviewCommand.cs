using Application.Models;
using MediatR;

namespace Application.ReportedReviews.Commands.UpdateReportedReview;

public sealed record UpdateReportedReviewCommand(
    long? ReportedReviewId,
    UpdateReportedReviewDto? ReportedReviewDto) : IRequest<UpdateReportedReviewDto>;