using FluentValidation;

namespace Application.Reviews.Commands.CreateReview;

public sealed class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(c => c.Rate)
            .NotNull()
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(5);

        RuleFor(c => c.Content)
            .Must(x => x is not { Length: > 1000 })
            .WithMessage("Review content can't have more than 1000 characters");

        RuleFor(c => c.FuelStationId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}