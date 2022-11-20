using FluentValidation;

namespace Application.Reviews.Commands.UpdateReview;

public sealed class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(u => u.ReviewId)
            .NotNull()
            .GreaterThanOrEqualTo(1);

        RuleFor(u => u.ReviewDto)
            .NotNull();

        When(x => x.ReviewDto is not null, () =>
        {
            RuleFor(u => u.ReviewDto!.Rate)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(5);
            
            RuleFor(c => c.ReviewDto!.Content)
                .Must(x => x is not { Length: > 1000 })
                .WithMessage("Review content can't have more than 1000 characters");
        });
    }
}