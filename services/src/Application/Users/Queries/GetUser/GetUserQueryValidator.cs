using FluentValidation;

namespace Application.Users.Queries.GetUser;

public sealed class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(q => q.Username)
            .NotEmpty();
    }
}