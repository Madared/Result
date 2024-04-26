namespace ResultAndOption.Errors;

/// <summary>
/// Allows for wrapping multiple errors into one formatting their messages
/// </summary>
public class MultipleErrors : IError
{
    private readonly IEnumerable<IError> _errors;

    public MultipleErrors(IEnumerable<IError> errors)
    {
        _errors = errors;
    }

    /// <summary>
    /// All error messages formatted into one
    /// </summary>
    public string Message => string.Format("{0} : \n {1}",
        "The following Errors have occurred",
        _errors
            .Select(error => error.Message)
            .Pipe(errorMessages => string.Join(",\n", errorMessages)));
}