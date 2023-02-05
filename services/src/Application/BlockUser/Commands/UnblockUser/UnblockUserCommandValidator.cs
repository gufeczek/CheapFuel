using FluentValidation;

namespace Application.BlockUser.Commands.UnblockUser;

public sealed class UnblockUserCommandValidator : AbstractValidator<UnblockUserCommand>
{
    public UnblockUserCommandValidator()
    {
        RuleFor(c => c.Username)
            .NotEmpty();
    }
}