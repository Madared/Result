namespace ResultAndOption.Errors;

/// <summary>
/// Any error which cannot be specified
/// </summary>
public sealed record UnknownError : CustomError
{
    private const string _message = "Unknown Error";

    /// <summary>
    /// Error message
    /// </summary>
    public override string Message => _message;
}