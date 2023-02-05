namespace Infrastructure.Identity;

public sealed class AuthenticationSettings
{
    public string? Secret { get; init; }
    public int? ExpireDays { get; init; }
    public string? Issuer { get; init; }
    public string? Audience { get; init; }
}