using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;
using ResultAndOption.Results;
using ResultAndOption.Results.Mappers;

namespace ResultAndOption.Results;

/// <summary>
/// A representation of the result of an action without internal data.
/// </summary>
public readonly struct Result : IResult
{
    private readonly CustomError? _error;
    
    /// <summary>
    /// Shows if the result has succeeded.
    /// </summary>
    public bool Succeeded { get; }
    
    /// <summary>
    /// Shows if the result has failed.
    /// </summary>
    public bool Failed => !Succeeded;

    /// <summary>
    /// Returns the error if the result has failed otherwise throws an exception.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public CustomError CustomError => Succeeded
        ? throw new InvalidOperationException()
        : _error ?? new UnknownError();

    private Result(bool failed, CustomError? error)
    {
        Succeeded = !failed;
        _error = error;
    }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="Result" /> struct representing a success.
    /// </summary>
    /// <returns>A new successful result.</returns>
    public static Result Ok() => new(false, null);

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result" /> struct representing a failure with the specified error.
    /// </summary>
    /// <param name="customError">The error associated with the failure.</param>
    /// <returns>A new failed result with the specified error.</returns>
    public static Result Fail(CustomError customError) => new(true, customError);
}