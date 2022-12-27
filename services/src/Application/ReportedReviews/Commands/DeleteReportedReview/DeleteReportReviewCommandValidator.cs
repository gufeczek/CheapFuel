using FluentValidation;

namespace Application.ReportedReviews.Commands.DeleteReportedReview;

public class DeleteReportReviewCommandValidator : AbstractValidator<DeleteReportReviewCommand>
{
    public DeleteReportReviewCommandValidator()
    {
        RuleFor(d => d.ReviewId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
        
        RuleFor(d => d.UserId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}