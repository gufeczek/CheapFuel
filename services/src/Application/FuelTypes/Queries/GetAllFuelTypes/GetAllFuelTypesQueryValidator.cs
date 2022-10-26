using Application.Models.Pagination;
using FluentValidation;

namespace Application.FuelTypes.Queries.GetAllFuelTypes;

public class GetAllFuelTypesQueryValidator : AbstractValidator<GetAllFuelTypesQuery>
{
    public GetAllFuelTypesQueryValidator()
    {
        RuleFor(g => g.PageRequestDto)
            .SetValidator(new PageRequestDtoValidator());
    }
}