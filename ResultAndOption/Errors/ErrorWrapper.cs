namespace ResultAndOption.Errors;

/// <summary>
/// Exception wrapping an Error
/// </summary>
[Serializable]
public class ErrorWrapper : Exception
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="error"></param>
    private ErrorWrapper(IError error) : base(error.Message)
    {
        Error = error;
    }

    /// <summary>
    /// Wrapped Error
    /// </summary>
    public IError Error { get; }

    /// <summary>
    /// Factory method to create an ErrorWrapper so it doesnt wrap an Exception Wrapper and just returns the internal exception
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Exception Create(IError error)
    {
        if (error is ExceptionWrapper ex) return ex.Exception;
        return new ErrorWrapper(error);
    }
}