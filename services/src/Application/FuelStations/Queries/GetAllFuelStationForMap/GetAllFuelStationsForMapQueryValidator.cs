using FluentValidation;

namespace Application.FuelStations.Queries.GetAllFuelStationForMap;

public sealed class GetAllFuelStationsForMapQueryValidator : AbstractValidator<GetAllFuelStationsForMapQuery>
{
    public GetAllFuelStationsForMapQueryValidator()
    {
        RuleFor(g => g.FilterDto)
            .NotNull();

        When(x => x.FilterDto is not null, () =>
        {
            RuleFor(g => g.FilterDto!.FuelTypeId)
                .NotNull()
                .GreaterThanOrEqualTo(1);

            RuleFor(g => g.FilterDto!.MinPrice)
                .GreaterThanOrEqualTo(0)
                .Must((model, minPrice) =>
                    minPrice is null ||
                    model.FilterDto.MaxPrice is null ||
                    minPrice <= model.FilterDto.MaxPrice)
                .WithMessage("Min price can't be greater than max price");

            RuleFor(g => g.FilterDto!.MaxPrice)
                .GreaterThanOrEqualTo(0);
        });
    }
}