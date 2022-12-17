using FluentValidation;

namespace Application.FuelAtStations.Commands.RemoveFuelFromStation;

public sealed class RemoveFuelFromStationCommandValidator : AbstractValidator<RemoveFuelFromStationCommand>
{
    public RemoveFuelFromStationCommandValidator()
    {
        RuleFor(c => c.FuelStationId)
            .NotNull()
            .GreaterThanOrEqualTo(1);

        RuleFor(c => c.FuelTypeId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}