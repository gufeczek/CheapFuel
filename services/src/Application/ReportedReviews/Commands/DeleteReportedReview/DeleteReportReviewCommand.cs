using MediatR;

namespace Application.ReportedReviews.Commands.DeleteReportedReview;

public sealed record DeleteReportReviewCommand(long? ReviewId, long? UserId) : IRequest<Unit>;
