namespace ResultAndOption.Errors;

/// <summary>
/// Wraps an exception to be used as an Error
/// </summary>
public record ExceptionWrapper : IError
{
    /// <summary>
    /// Wrapped exception
    /// </summary>
    public Exception Exception { get; }
    
    public ExceptionWrapper(Exception exception)
    {
        Exception = exception;
    }

    /// <summary>
    /// Internal exception message
    /// </summary>
    public string Message => Exception.Message;
}