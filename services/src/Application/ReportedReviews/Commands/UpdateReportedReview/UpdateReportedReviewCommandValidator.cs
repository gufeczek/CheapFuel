using AutoMapper;
using Domain.Entities.Tokens;
using FluentValidation;

namespace Application.ReportedReviews.Commands.UpdateReportedReview;

public class UpdateReportedReviewCommandValidator : AbstractValidator<UpdateReportedReviewCommand>
{
    public UpdateReportedReviewCommandValidator()
    {
        RuleFor(r => r.ReportedReviewId)
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