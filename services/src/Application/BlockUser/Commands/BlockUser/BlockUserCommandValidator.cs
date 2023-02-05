using FluentValidation;

namespace Application.BlockUser.Commands.BlockUser;

public sealed class BlockUserCommandValidator : AbstractValidator<BlockUserCommand>
{
    public BlockUserCommandValidator()
    {
        RuleFor(c => c.Username)
            .NotEmpty();

        RuleFor(c => c.Reason) 
            .NotNull() 
            .MaximumLength(100);
    }
}