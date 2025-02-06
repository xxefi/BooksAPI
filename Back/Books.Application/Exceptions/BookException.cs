namespace Books.Application.Exceptions;

public class BookException : Exception
{
    public ExceptionType ExceptionType { get; set; }
    public BookException(ExceptionType exceptionType, string message) : base(message)
        => ExceptionType = exceptionType;
}