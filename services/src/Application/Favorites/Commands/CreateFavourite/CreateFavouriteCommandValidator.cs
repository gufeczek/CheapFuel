using FluentValidation;

namespace Application.Favorites.Commands.CreateFavourite;

public sealed class CreateFavouriteCommandValidator : AbstractValidator<CreateFavouriteCommand>
{
    public CreateFavouriteCommandValidator()
    {
        RuleFor(c => c.FuelStationId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}