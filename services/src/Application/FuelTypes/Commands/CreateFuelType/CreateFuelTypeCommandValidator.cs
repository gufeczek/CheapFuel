using FluentValidation;

namespace Application.FuelTypes.Commands.CreateFuelType;

public sealed class CreateFuelTypeCommandValidator : AbstractValidator<CreateFuelTypeCommand>
{
    public CreateFuelTypeCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(32);
    }
}