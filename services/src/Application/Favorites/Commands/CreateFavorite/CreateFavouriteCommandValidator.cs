using FluentValidation;

namespace Application.Favorites.Commands.CreateFavorite;

public sealed class CreateFavoriteCommandValidator : AbstractValidator<CreateFavouriteCommand>
{
    public CreateFavoriteCommandValidator()
    {
        RuleFor(c => c.FuelStationId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}