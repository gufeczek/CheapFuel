namespace WebAPI.Models;

public record PropertyValidationFailure(string? Property, string[]? Errors);