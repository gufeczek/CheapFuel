using FluentValidation;

namespace Application.Models.Pagination;

public sealed class PageRequestDtoValidator : AbstractValidator<PageRequestDto>
{
    public PageRequestDtoValidator()
    {
        RuleFor(p => p.PageSize)
            .GreaterThan(0);

        RuleFor(p => p.PageNumber)
            .GreaterThan(0);
    }
}