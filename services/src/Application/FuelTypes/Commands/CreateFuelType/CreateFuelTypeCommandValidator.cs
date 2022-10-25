using FluentValidation;

namespace Application.FuelTypes.Commands.CreateFuelType;

public class CreateFuelTypeCommandValidator : AbstractValidator<CreateFuelTypeCommand>
{
    public CreateFuelTypeCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(32);
    }
}