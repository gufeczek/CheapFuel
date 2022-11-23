using FluentValidation;

namespace Application.Favorites.Commands.DeleteFavorite;

public sealed class DeleteFavoriteCommandValidator : AbstractValidator<DeleteFavoriteCommand>
{
    public DeleteFavoriteCommandValidator()
    {
        RuleFor(d => d.FuelStationId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}