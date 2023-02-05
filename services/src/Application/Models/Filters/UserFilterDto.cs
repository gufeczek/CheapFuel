using Domain.Enums;

namespace Application.Models.Filters;

public sealed record UserFilterDto(
    Role? Role, 
    AccountStatus? AccountStatus, 
    string? SearchPhrase);