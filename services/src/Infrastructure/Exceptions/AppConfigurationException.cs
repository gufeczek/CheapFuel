namespace Infrastructure.Exceptions;

public class AppConfigurationException : Exception
{
    public AppConfigurationException(string message) : base(message) { }
}