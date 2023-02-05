namespace Infrastructure.Common.Services.Email;

public class EmailHostSettings
{
    public string? EmailAddress { get; init; }
    public string? Password { get; init; }
    public string? Host { get; set; }
    public int? Port { get; set; }
}