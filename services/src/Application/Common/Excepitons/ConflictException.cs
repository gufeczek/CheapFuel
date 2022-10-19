namespace Application.Excepitons;

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
        
    }
}