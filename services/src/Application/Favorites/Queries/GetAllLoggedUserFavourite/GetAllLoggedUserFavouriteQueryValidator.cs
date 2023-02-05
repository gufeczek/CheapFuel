using Application.Models.Pagination;
using FluentValidation;

namespace Application.Favorites.Queries.GetAllLoggedUserFavourite;

public sealed class GetAllLoggedUserFavouriteQueryValidator : AbstractValidator<GetAllLoggedUserFavouriteQuery>
{
    public GetAllLoggedUserFavouriteQueryValidator()
    {
        RuleFor(g => g.PageRequestDto)
            .SetValidator(new PageRequestDtoValidator());
    }
}