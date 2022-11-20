using Application.Models.Pagination;
using FluentValidation;

namespace Application.Users.Queries.GetAllUsers;

public class GetAllUserQueryValidator : AbstractValidator<GetAllUsers.GetAllUsersQuery>
{
    public GetAllUserQueryValidator()
    {
        RuleFor(g => g.PageRequestDto)
            .SetValidator(new PageRequestDtoValidator());
    }
}