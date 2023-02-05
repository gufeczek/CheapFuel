using FluentValidation;

namespace Application.OwnerStations.Commands.CreateFuelStationOwner;

public sealed class CreateFuelStationOwnerCommandValidator : AbstractValidator<CreateFuelStationOwnerCommand>
{
    public CreateFuelStationOwnerCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotNull()
            .GreaterThanOrEqualTo(1);

        RuleFor(c => c.FuelStationId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}