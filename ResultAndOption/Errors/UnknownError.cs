namespace ResultAndOption.Errors;

/// <summary>
/// Any error which cannot be specified
/// </summary>
public class UnknownError : IError {
    private const string _message = "Unknown Error";
    /// <summary>
    /// Error message
    /// </summary>
    public string Message => _message;
}