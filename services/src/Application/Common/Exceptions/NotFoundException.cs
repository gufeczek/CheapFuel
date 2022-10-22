namespace Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string entityName, string paramName, string value) 
        : base($"{entityName} not found for {paramName} = {value}") { }
}