using FluentValidation;

namespace Application.FuelStations.Queries.GetMostEconomicalFuelStation;

public class GetMostEconomicalFuelStationQueryValidator : AbstractValidator<GetMostEconomicalFuelStationQuery>
{
    public GetMostEconomicalFuelStationQueryValidator()
    {
        RuleFor(g => g.UserLatitude)
            .NotNull()
            .GreaterThanOrEqualTo(-90.0)
            .LessThanOrEqualTo(90.0);
            
        RuleFor(g => g.UserLongitude)
            .NotNull()
            .GreaterThanOrEqualTo(-180.0)
            .LessThanOrEqualTo(180.0);

        RuleFor(g => g.FuelConsumption)
            .NotNull()
            .GreaterThanOrEqualTo(0.0);

        RuleFor(g => g.FuelAmountToBuy)
            .NotNull()
            .GreaterThanOrEqualTo(0.0);

        RuleFor(g => g.FuelTypeId)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}