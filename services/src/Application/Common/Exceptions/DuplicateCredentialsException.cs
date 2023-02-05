namespace Application.Common.Exceptions;

public class DuplicateCredentialsException : ConflictException
{
    public DuplicateCredentialsException(string message) : base(message) { }
}