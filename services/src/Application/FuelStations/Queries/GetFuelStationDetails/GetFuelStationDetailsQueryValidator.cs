using FluentValidation;

namespace Application.FuelStations.Queries.GetFuelStationDetails;

public sealed class GetFuelStationDetailsQueryValidator : AbstractValidator<GetFuelStationDetailsQuery>
{
    public GetFuelStationDetailsQueryValidator()
    {
        RuleFor(c => c.Id)
            .NotNull()
            .GreaterThan(0);
    }
}