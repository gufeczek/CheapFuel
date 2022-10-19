using FluentValidation;

namespace Application.StationChains.Commands.CreateStationChain;

public class CreateStationChainValidator : AbstractValidator<CreateStationChainCommand>
{
    public CreateStationChainValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(128);
    }
}