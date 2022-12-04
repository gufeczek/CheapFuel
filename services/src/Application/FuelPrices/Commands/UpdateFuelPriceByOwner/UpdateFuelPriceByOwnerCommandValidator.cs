using Application.Models.FuelPriceDtos;
using FluentValidation;

namespace Application.FuelPrices.Commands.UpdateFuelPriceByOwner;

public sealed class UpdateFuelPriceByOwnerCommandValidator : AbstractValidator<UpdateFuelPriceByOwnerCommand>
{
    public UpdateFuelPriceByOwnerCommandValidator()
    {
        RuleFor(u => u.FuelPricesAtStationDto)
            .NotNull();

        When(x => x.FuelPricesAtStationDto is not null, () =>
        {
            RuleFor(u => u.FuelPricesAtStationDto!.FuelStationId)
                .NotNull()
                .GreaterThanOrEqualTo(1);

            RuleFor(u => u.FuelPricesAtStationDto!.FuelPrices)
                .NotEmpty();
            
            RuleForEach(u => u.FuelPricesAtStationDto!.FuelPrices)
                .SetValidator(new NewFuelPriceDtoValidator());
        });
    }
}

public sealed class NewFuelPriceDtoValidator : AbstractValidator<NewFuelPriceDto>
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