using FluentValidation;

namespace Application.FuelStationServices.Commands.DeleteFuelStationService;

public sealed class DeleteFuelStationServiceCommandValidator : AbstractValidator<DeleteFuelStationServiceCommand>
{
    public DeleteFuelStationServiceCommandValidator()
    {
        RuleFor(d => d.Id)
            .NotNull()
            .GreaterThan(0);
    }
}