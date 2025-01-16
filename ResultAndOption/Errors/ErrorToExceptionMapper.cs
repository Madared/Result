namespace ResultAndOption.Errors;

/// <summary>
/// Holds Map method from IErrors to Exceptions
/// </summary>
public static class ErrorToExceptionMapper
{
    /// <summary>
    /// Maps any error or null to a specific Exception
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Exception Map(CustomError? error) => error switch
    {
        null => new InvalidOperationException(),
        ExceptionWrapper e => e.Exception,
        _ => ErrorWrapper.Create(error)
    };
}