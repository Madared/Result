namespace ResultAndOption.Errors;

/// <summary>
/// Wraps an exception to be used as an Error
/// </summary>
public record ExceptionWrapper : CustomError
{
    /// <summary>
    /// Wrapped exception
    /// </summary>
    public Exception Exception { get; }
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="exception"></param>
    public ExceptionWrapper(Exception exception)
    {
        Exception = exception;
    }

    /// <summary>
    /// Internal exception message
    /// </summary>
    public override string Message => Exception.Message;
}