using FluentValidation;

namespace Application.Users.Commands.DeactivateUser;

public class DeactivateUserCommandValidator : AbstractValidator<DeactivateUserCommand>
{
    public DeactivateUserCommandValidator()
    {
        RuleFor(d => d.Username)
            .NotEmpty();
    }
}