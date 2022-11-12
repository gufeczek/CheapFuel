namespace Application.Common.Exceptions;

public class FilterValidationException : Exception
{
    public IEnumerable<string> ValidationErrors { get; }

    public FilterValidationException(IEnumerable<string> validationErrors)
    {
        ValidationErrors = validationErrors;
    }
}