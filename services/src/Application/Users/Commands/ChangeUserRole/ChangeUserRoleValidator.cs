using FluentValidation;

namespace Application.Users.Commands.ChangeUserRole;

public class ChangeUserRoleValidator : AbstractValidator<ChangeUserRoleCommand>
{
    public ChangeUserRoleValidator()
    {
        RuleFor(c => c.Username)
            .NotEmpty();

        RuleFor(c => c.Role)
            .NotNull();
    }
}