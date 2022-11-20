using MediatR;

namespace Application.Reviews.Commands.DeleteReview;

public sealed record DeleteReviewCommand(long? ReviewId) : IRequest<Unit>;