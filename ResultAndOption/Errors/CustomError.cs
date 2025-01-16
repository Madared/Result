namespace ResultAndOption.Errors;

/// <summary>
/// Error interface
/// </summary>
public abstract record CustomError
{
    /// <summary>
    /// Internal error message
    /// </summary>
    public abstract string Message { get; }
}