namespace ResultAndOption.Errors;

/// <summary>
/// Error interface
/// </summary>
public interface IError
{
    /// <summary>
    /// Internal error message
    /// </summary>
    public string Message { get; }
}