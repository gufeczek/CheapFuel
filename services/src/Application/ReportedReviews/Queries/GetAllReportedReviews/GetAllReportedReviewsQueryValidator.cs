using Application.Models.Pagination;
using FluentValidation;

namespace Application.ReportedReviews.Queries.GetAllReportedReviews;

public class GetAllReportedReviewsQueryValidator : AbstractValidator<GetAllReportedReviewsQuery>
{
    public GetAllReportedReviewsQueryValidator()
    {
        RuleFor(g => g.PageRequestDto)
            .SetValidator(new PageRequestDtoValidator());
    }
}