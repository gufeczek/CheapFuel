using FluentValidation;

namespace Application.FuelTypes.Commands.DeleteFuelType;

public sealed class DeleteFuelTypeCommandValidator : AbstractValidator<DeleteFuelTypeCommand>
{
    public DeleteFuelTypeCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotNull()
            .GreaterThan(0);
    }
}