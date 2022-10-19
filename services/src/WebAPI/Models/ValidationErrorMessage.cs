namespace WebAPI.Models;

public record ValidationErrorMessage : ErrorMessage
{
    public PropertyValidationFailure[] Violations { get; init; }
}