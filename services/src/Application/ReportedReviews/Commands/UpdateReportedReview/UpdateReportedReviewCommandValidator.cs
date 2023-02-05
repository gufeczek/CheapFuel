using FluentValidation;

namespace Application.ReportedReviews.Commands.UpdateReportedReview;

public class UpdateReportedReviewCommandValidator : AbstractValidator<UpdateReportedReviewCommand>
{
    public UpdateReportedReviewCommandValidator()
    {
        RuleFor(r => r.ReviewId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
        
        RuleFor(r => r.UserId)
            .NotNull()
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.ReportedReviewDto)
            .NotNull();

        When(x => x.ReportedReviewDto is not null, () =>
        {
            RuleFor(u => u.ReportedReviewDto!.ReportStatus)
                .NotNull()
                .IsInEnum();
        });
    }
}