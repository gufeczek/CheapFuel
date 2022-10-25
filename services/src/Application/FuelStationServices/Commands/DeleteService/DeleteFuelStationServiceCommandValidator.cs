using FluentValidation;

namespace Application.FuelStationServices.Commands.DeleteService;

public class DeleteFuelStationServiceCommandValidator : AbstractValidator<DeleteFuelStationServiceCommand>
{
    public DeleteFuelStationServiceCommandValidator()
    {
        RuleFor(d => d.Id)
            .NotNull()
            .GreaterThan(0);
    }
}