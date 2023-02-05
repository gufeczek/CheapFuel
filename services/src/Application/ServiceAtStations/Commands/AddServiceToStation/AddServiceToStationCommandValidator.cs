using FluentValidation;

namespace Application.ServiceAtStations.Commands.AddServiceToStation;

public sealed class AddServiceToStationCommandValidator : AbstractValidator<AddServiceToStationCommand>
{
    public AddServiceToStationCommandValidator()
    {
        RuleFor(c => c.FuelStationId)
            .NotNull()
            .GreaterThanOrEqualTo(1);

        RuleFor(c => c.ServiceId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}