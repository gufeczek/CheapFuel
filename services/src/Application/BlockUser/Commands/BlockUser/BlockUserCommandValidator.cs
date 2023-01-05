using FluentValidation;

namespace Application.BlockUser.Commands.BlockUser;

public sealed class BlockUserCommandValidator : AbstractValidator<BlockUserCommand>
{
    public BlockUserCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotNull()
            .GreaterThanOrEqualTo(1);

        RuleFor(c => c.Reason)
            .NotNull()
            .Must(c => c is not { Length: > 1000 })
            .WithMessage("Reason content can't have more than 1000 characters");

    }
}