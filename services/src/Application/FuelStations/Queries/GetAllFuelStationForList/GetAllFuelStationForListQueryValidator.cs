using Application.Models.Pagination;
using FluentValidation;

namespace Application.FuelStations.Queries.GetAllFuelStationForList;

public class GetAllFuelStationForListQueryValidator : AbstractValidator<GetAllFuelStationForListQuery>
{
    public GetAllFuelStationForListQueryValidator()
    {
        RuleFor(g => g.Filter)
            .NotNull();

        When(x => x.Filter is not null, () =>
        {
            RuleFor(g => g.Filter!.FuelTypeId)
                .NotNull()
                .GreaterThanOrEqualTo(1);

            RuleFor(g => g.Filter!.MinPrice)
                .GreaterThanOrEqualTo(0)
                .Must((model, minPrice) =>
                    minPrice is null ||
                    model.Filter.MaxPrice is null ||
                    minPrice <= model.Filter.MaxPrice)
                .WithMessage("Min price can't be greater than max price");

            RuleFor(g => g.Filter!.MaxPrice)
                .GreaterThanOrEqualTo(0);
            
            RuleFor(g => g.Filter!.UserLatitude)
                .GreaterThanOrEqualTo(-90.0)
                .LessThanOrEqualTo(90.0);
            
            RuleFor(g => g.Filter!.UserLongitude)
                .GreaterThanOrEqualTo(-180.0)
                .LessThanOrEqualTo(180.0);

            RuleFor(g => g.Filter!.Distance)
                .GreaterThanOrEqualTo(0);
        });

        RuleFor(g => g.PageRequest)
            .NotNull()
            .SetValidator(new PageRequestDtoValidator());
    }
}