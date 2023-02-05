using FluentValidation;

namespace Application.ServiceAtStations.Commands.RemoveServiceFromStation;

public sealed class RemoveServiceFromStationCommandValidator : AbstractValidator<RemoveServiceFromStationCommand>
{
    public RemoveServiceFromStationCommandValidator()
    {
        RuleFor(c => c.FuelStationId)
            .NotNull()
            .GreaterThanOrEqualTo(1);

        RuleFor(c => c.ServiceId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }    
}