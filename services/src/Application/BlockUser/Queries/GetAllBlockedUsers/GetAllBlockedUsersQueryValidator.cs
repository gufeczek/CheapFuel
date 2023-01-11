using Application.Models.Pagination;
using FluentValidation;

namespace Application.BlockUser.Queries.GetAllBlockedUsers;

public class GetAllBlockedUsersQueryValidator : AbstractValidator<GetAllBlockedUsersQuery>
{
    public GetAllBlockedUsersQueryValidator()
    {
        RuleFor(g => g.PageRequestDto)
            .SetValidator(new PageRequestDtoValidator());
    }
}