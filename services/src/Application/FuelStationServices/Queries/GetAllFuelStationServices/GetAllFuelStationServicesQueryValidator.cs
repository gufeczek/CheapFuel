using Application.Models.Pagination;
using FluentValidation;

namespace Application.FuelStationServices.Queries.GetAllFuelStationServices;

public class GetAllFuelStationServicesQueryValidator : AbstractValidator<GetAllFuelStationServicesQuery>
{
    public GetAllFuelStationServicesQueryValidator()
    {
        RuleFor(g => g.PageRequestDto)
            .SetValidator(new PageRequestDtoValidator());
    }
}