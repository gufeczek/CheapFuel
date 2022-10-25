using FluentValidation;

namespace Application.FuelStationServices.Commands.CreateFuelStationService;

public sealed class CreateFuelStationServiceCommandValidator : AbstractValidator<CreateFuelStationServiceCommand>
{
    public CreateFuelStationServiceCommandValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(32);
    }
}