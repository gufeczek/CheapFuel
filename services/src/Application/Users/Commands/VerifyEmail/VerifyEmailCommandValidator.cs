using FluentValidation;

namespace Application.Users.Commands.VerifyEmail;

public sealed class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(v => v.Token)
            .NotEmpty()
            .Length(6);
    }
}