using Application.Models.Pagination;
using FluentValidation;

namespace Application.StationChains.Queries.GetAllStationChainsQuery;

public class GetAllStationChainsQueryValidator : AbstractValidator<GetAllStationChainsQuery>
{
    public GetAllStationChainsQueryValidator() 
    {
        RuleFor(g => g.PageRequestDto)
            .SetValidator(new PageRequestDtoValidator());
    }
}