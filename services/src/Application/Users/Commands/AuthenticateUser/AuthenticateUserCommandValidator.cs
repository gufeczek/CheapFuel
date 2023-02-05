using FluentValidation;

namespace Application.Users.Commands.AuthenticateUser;

public sealed class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
{
    public AuthenticateUserCommandValidator()
    {
        RuleFor(a => a.Username)
            .NotEmpty();

        RuleFor(a => a.Password)
            .NotEmpty();
    }
}