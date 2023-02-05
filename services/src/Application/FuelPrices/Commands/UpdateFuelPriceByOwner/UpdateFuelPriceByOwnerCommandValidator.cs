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