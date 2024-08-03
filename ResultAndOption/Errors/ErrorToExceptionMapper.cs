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
    public static Exception Map(IError? error) => error switch
    {
        null => new InvalidOperationException(),
        // ReSharper disable once SuspiciousTypeConversion.Global
        Exception e => e,
        ExceptionWrapper e => e.Exception,
        _ => ErrorWrapper.Create(error)
    };
}