using Application.Models.FuelPriceDtos;
using FluentValidation;

namespace Application.FuelPrices.Commands;

public class NewFuelPriceDtoValidator : AbstractValidator<NewFuelPriceDto>
{
    public NewFuelPriceDtoValidator()
    {
        RuleFor(n => n.Available)
            .NotNull();

        RuleFor(n => n.Price)
            .NotNull()
            .GreaterThanOrEqualTo(0.0M);

        RuleFor(n => n.FuelTypeId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}