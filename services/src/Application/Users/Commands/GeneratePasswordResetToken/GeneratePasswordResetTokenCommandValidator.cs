using FluentValidation;

namespace Application.Users.Commands.GeneratePasswordResetToken;

public sealed class GeneratePasswordResetTokenCommandValidator : AbstractValidator<GeneratePasswordResetTokenCommand>
{
    public GeneratePasswordResetTokenCommandValidator()
    {
        RuleFor(g => g.Email)
            .NotEmpty()
            .EmailAddress();
    }
}