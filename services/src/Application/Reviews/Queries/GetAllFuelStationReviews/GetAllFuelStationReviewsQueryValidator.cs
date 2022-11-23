using Application.Models.Pagination;
using FluentValidation;

namespace Application.Reviews.Queries.GetAllFuelStationReviews;

public class GetAllFuelStationReviewsQueryValidator : AbstractValidator<GetAllFuelStationReviewsQuery>
{
    public GetAllFuelStationReviewsQueryValidator()
    {
        RuleFor(g => g.Id)
            .NotNull()
            .GreaterThan(0);

        RuleFor(g => g.PageRequestDto)
            .SetValidator(new PageRequestDtoValidator());
    }
}