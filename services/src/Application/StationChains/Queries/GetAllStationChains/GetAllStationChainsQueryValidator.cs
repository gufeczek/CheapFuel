using Application.Models.Pagination;
using FluentValidation;

namespace Application.StationChains.Queries.GetAllStationChains;

public class GetAllStationChainsQueryValidator : AbstractValidator<GetAllStationChains.GetAllStationChainsQuery>
{
    public GetAllStationChainsQueryValidator() 
    {
        RuleFor(g => g.PageRequestDto)
            .SetValidator(new PageRequestDtoValidator());
    }
}