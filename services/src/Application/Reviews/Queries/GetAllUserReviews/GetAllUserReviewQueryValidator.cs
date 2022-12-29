using Application.Models.Pagination;
using FluentValidation;

namespace Application.Reviews.Queries.GetAllUserReviews;

public class GetAllUserReviewQueryValidator : AbstractValidator<GetAllUserReviewsQuery>
{
    public GetAllUserReviewQueryValidator()
    {
        RuleFor(g => g.Username)
            .NotEmpty();

        RuleFor(g => g.PageRequest)
            .SetValidator(new PageRequestDtoValidator());
    }
}