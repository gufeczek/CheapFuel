using FluentValidation;

namespace Application.Reviews.Queries.GetUserReviewOfFuelStation;

public sealed class GetUserReviewOfFuelStationQueryValidator : AbstractValidator<GetUserReviewOfFuelStationQuery>
{
    public GetUserReviewOfFuelStationQueryValidator()
    {
        RuleFor(g => g.Username)
            .NotEmpty();
        
        RuleFor(g => g.FuelStationId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}