namespace WebAPI.Models;

public record PropertyValidationFailure
{
    public string Property { get; init; }
    public string[] Errors { get; init; }
}