using FluentValidation;

namespace Application.Users.Commands.ChangeUserRole;

public sealed class ChangeUserRoleCommandValidator : AbstractValidator<ChangeUserRoleCommand>
{
    public ChangeUserRoleCommandValidator()
    {
        RuleFor(c => c.Username)
            .NotEmpty();

        RuleFor(c => c.Role)
            .NotNull();
    }
}