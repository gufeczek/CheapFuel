using System.Net;

namespace WebAPI.Models;

public record ErrorMessage
{
    public HttpStatusCode StatusCode { get; set; }
    public string Title { get; init; } = string.Empty;
    public string Details { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
}