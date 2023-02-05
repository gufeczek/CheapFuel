using FluentValidation;

namespace Application.Users.Queries.GetUserForAdministration;

public sealed class GetUserForAdministrationQueryValidator : AbstractValidator<GetUserForAdministrationQuery>
{
    public GetUserForAdministrationQueryValidator()
    {
        RuleFor(q => q.Username)
            .NotEmpty();
    }
}