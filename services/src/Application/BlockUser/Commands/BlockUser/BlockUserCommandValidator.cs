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
            .MaximumLength(100);
    }
}