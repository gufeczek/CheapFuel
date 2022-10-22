using System.Net;

namespace WebAPI.Models;

public record ErrorMessage(HttpStatusCode StatusCode, string Title, string Details, DateTime Timestamp);