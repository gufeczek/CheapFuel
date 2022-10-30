using System.Runtime.CompilerServices;

namespace Application.Common;

public static class NullExtension
{
    public static T OrElseThrow<T>(
        this T? arg, 
        string? message = default,
        [CallerArgumentExpression("arg")] string? paramName = default
        ) where T : notnull
    {
        if (arg is null)
        {
            throw new ArgumentNullException(paramName, message);
        }

        return arg;
    }
    
    public static T OrElseThrow<T>(
        this T? arg, 
        string? message = default,
        [CallerArgumentExpression("arg")] string? paramName = default
    ) where T : unmanaged
    {
        if (arg is null)
        {
            throw new ArgumentNullException(paramName, message);
        }

        return (T) arg;
    }
}