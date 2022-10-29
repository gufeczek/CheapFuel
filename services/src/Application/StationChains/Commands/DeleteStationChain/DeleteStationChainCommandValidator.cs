using FluentValidation;

namespace Application.StationChains.Commands.DeleteStationChain;

public sealed class DeleteStationChainCommandValidator : AbstractValidator<DeleteStationChainCommand>
{
    public DeleteStationChainCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotNull()
            .GreaterThan(0);
    }
}