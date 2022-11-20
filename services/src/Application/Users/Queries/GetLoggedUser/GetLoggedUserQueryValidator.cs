using FluentValidation;

namespace Application.Users.Queries.GetLoggedUser;

public sealed class GetLoggedUserQueryValidator : AbstractValidator<GetLoggedUserQuery>
{
    public GetLoggedUserQueryValidator()
    {}
}