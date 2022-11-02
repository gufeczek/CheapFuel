using FluentValidation;

namespace Application.Users.Commands.GeneratePasswordResettingToken;

public class GeneratePasswordResettingTokenValidator : AbstractValidator<GeneratePasswordResettingTokenCommand>
{
    public GeneratePasswordResettingTokenValidator()
    {
        RuleFor(g => g.Email)
            .EmailAddress();
    }
}