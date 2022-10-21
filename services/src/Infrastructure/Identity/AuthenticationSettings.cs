namespace Infrastructure.Identity;

public class AuthenticationSettings
{
    public string? JwtKey { get; init; }
    public int JwtExpireDays { get; init; }
    public string? JwtIssuer { get; init; }
}