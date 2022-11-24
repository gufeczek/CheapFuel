using FluentValidation;

namespace Application.Favorites.Commands.DeleteFavourite;

public sealed class DeleteFavouriteCommandValidator : AbstractValidator<DeleteFavouriteCommand>
{
    public DeleteFavouriteCommandValidator()
    {
        RuleFor(d => d.FuelStationId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}