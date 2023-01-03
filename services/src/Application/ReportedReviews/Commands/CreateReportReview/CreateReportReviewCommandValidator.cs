using FluentValidation;

namespace Application.ReportedReviews.Commands.CreateReportReview;

public sealed class CreateReportReviewCommandValidator : AbstractValidator<CreateReportReviewCommand>
{
    public CreateReportReviewCommandValidator()
    {
        RuleFor(c => c.ReviewId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}