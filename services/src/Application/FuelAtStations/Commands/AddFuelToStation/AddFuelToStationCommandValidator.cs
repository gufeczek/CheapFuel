using FluentValidation;

namespace Application.FuelAtStations.Commands.AddFuelToStation;

public sealed class AddFuelToStationCommandValidator : AbstractValidator<AddFuelToStationCommand>
{
    public AddFuelToStationCommandValidator()
    {
        RuleFor(c => c.FuelStationId)
            .NotNull()
            .GreaterThanOrEqualTo(1);

        RuleFor(c => c.FuelTypeId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}