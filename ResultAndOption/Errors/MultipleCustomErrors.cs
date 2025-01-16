namespace ResultAndOption.Errors;

/// <summary>
/// Allows for wrapping multiple errors into one formatting their messages
/// </summary>
public sealed record MultipleCustomErrors : CustomError
{
    /// <summary>
    /// Collection of errors.
    /// </summary>
    public IEnumerable<CustomError> Errors { get; }

    /// <summary>
    /// Public constructor
    /// </summary>
    /// <param name="errors"></param>
    public MultipleCustomErrors(IEnumerable<CustomError> errors)
    {
        Errors = errors;
    }

    /// <summary>
    /// All error messages formatted into one
    /// </summary>
    public override string Message => string.Format("{0} : \n {1}",
        "The following Errors have occurred",
        Errors
            .Select(error => error.Message)
            .Pipe(errorMessages => string.Join(",\n", errorMessages)));
}