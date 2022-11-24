using FluentValidation;

namespace Application.Favorites.Queries.GetFavourite;

public sealed class GetFavouriteQueryValidator : AbstractValidator<GetFavouriteQuery>
{
    public GetFavouriteQueryValidator()
    {
        RuleFor(g => g.Username)
            .NotEmpty();

        RuleFor(g => g.FuelStationId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}