using FluentValidation;

namespace Application.Reviews.Commands.DeleteReview;

public sealed class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
{
    public DeleteReviewCommandValidator()
    {
        RuleFor(d => d.ReviewId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}