namespace ResultAndOption.Errors;

/// <summary>
/// Exception wrapping an Error
/// </summary>
[Serializable]
public sealed class ErrorWrapper : Exception
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="customError"></param>
    private ErrorWrapper(CustomError customError) : base(customError.Message)
    {
        CustomError = customError;
    }

    /// <summary>
    /// Wrapped Error
    /// </summary>
    public CustomError CustomError { get; }

    /// <summary>
    /// Factory method to create an ErrorWrapper so it doesnt wrap an Exception Wrapper and just returns the internal exception
    /// </summary>
    /// <param name="customError"></param>
    /// <returns></returns>
    public static Exception Create(CustomError customError)
    {
        if (customError is ExceptionWrapper ex) return ex.Exception;
        return new ErrorWrapper(customError);
    }
}