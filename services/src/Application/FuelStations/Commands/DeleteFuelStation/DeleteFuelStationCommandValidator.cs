using FluentValidation;

namespace Application.FuelStations.Commands.DeleteFuelStation;

public sealed class DeleteFuelStationCommandValidator : AbstractValidator<DeleteFuelStationCommand>
{
    public DeleteFuelStationCommandValidator()
    {
        RuleFor(d => d.FuelStationId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}