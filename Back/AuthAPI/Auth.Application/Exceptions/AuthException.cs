namespace Auth.Application.Exceptions;

public class AuthException : Exception
{
    public ExceptionType ExceptionType { get; set; }
    public AuthException(ExceptionType exceptionType, string message) : base(message)
        => ExceptionType = exceptionType;
}