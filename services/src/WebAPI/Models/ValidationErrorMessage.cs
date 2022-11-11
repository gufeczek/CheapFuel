using System.Net;

namespace WebAPI.Models;

public record ValidationErrorMessage(HttpStatusCode StatusCode, 
    string Title, 
    string Details, 
    DateTime Timestamp, 
    PropertyValidationFailure[] Violations) 
    : ErrorMessage(StatusCode, Title, Details, Timestamp);