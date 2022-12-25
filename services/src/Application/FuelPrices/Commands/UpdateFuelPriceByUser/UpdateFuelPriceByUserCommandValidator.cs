using FluentValidation;

namespace Application.FuelPrices.Commands.UpdateFuelPriceByUser;

public sealed class UpdateFuelPriceByUserCommandValidator : AbstractValidator<UpdateFuelPriceByUserCommand>
{
    public UpdateFuelPriceByUserCommandValidator()
    {
        RuleFor(u => u.FuelPricesAtStation)
            .NotNull();

        When(x => x.FuelPricesAtStation is not null, () =>
        {
            RuleFor(u => u.FuelPricesAtStation!.FuelStationId)
                .NotNull()
                .GreaterThanOrEqualTo(1);

            RuleFor(u => u.FuelPricesAtStation!.FuelPrices)
                .NotEmpty();
            
            RuleForEach(u => u.FuelPricesAtStation!.FuelPrices)
                .SetValidator(new NewFuelPriceDtoValidator());
            
            RuleFor(g => g.FuelPricesAtStation!.UserLatitude)
                .GreaterThanOrEqualTo(-90.0)
                .LessThanOrEqualTo(90.0);
            
            RuleFor(g => g.FuelPricesAtStation!.UserLongitude)
                .GreaterThanOrEqualTo(-180.0)
                .LessThanOrEqualTo(180.0);
        });
    }
}