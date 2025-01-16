using ResultAndOption.Errors;
using ResultAndOption.Options;
using ResultAndOption.Results.Commands;
using ResultAndOption.Results.Mappers;

namespace ResultAndOption.Results;

/// <summary>
///     Represents a result of an operation that can either succeed or fail, carrying either data or an error.
/// </summary>
/// <typeparam name="T">The type of data carried by the result.</typeparam>
public readonly struct Result<T> : IResult<T> where T : notnull
{
    private readonly Option<T> _data;
    private readonly CustomError? _error;

    /// <summary>
    /// Returns the data from a successful result or throws the present error wrapped in an exception if failed.
    /// </summary>
    /// <exception cref="Exception"></exception>
    public T Data => _data.IsNone()
        ? throw ErrorToExceptionMapper.Map(_error)
        : _data.Data;

    /// <summary>
    /// Shows if result has failed
    /// </summary>
    public bool Failed => !Succeeded;

    /// <summary>
    /// Shows if result has succeeded
    /// </summary>
    public bool Succeeded { get; }
    private Result(bool failed, CustomError? error, Option<T> data)
    {
        Succeeded = !failed;
        _error = error;
        _data = data;
    }

    /// <summary>
    /// Returns the error from a failed result or throws an exception in a successful result.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public CustomError CustomError => Succeeded
        ? throw new InvalidOperationException("Cannot access Error on success Result!")
        : _error ?? new UnknownError();
 
    /// <summary>
    ///     Creates a successful result with the specified data.
    /// </summary>
    /// <param name="data">The data to carry.</param>
    /// <returns>A successful result carrying the specified data.</returns>
    public static Result<T> Ok(T data) => data is null
        ? throw new InvalidOperationException()
        : new Result<T>(false, null, Option<T>.Some(data));

    /// <summary>
    ///     Creates a failed result with the specified error.
    /// </summary>
    /// <param name="customError">The error associated with the failure.</param>
    /// <returns>A failed result with the specified error.</returns>
    public static Result<T> Fail(CustomError customError) => new(true, customError, Option<T>.None());

    /// <summary>
    ///     Creates a result with the specified data and error, where the success or failure depends on the provided data.
    /// </summary>
    /// <param name="data">The data associated with the result.</param>
    /// <param name="customError">The error to associate in case of null data</param>
    /// <returns>A result with either the specified data or error. The success or failure depends on the provided data.</returns>
    public static Result<T> Unknown(T? data, CustomError customError) => data is null
        ? Fail(customError)
        : Ok(data);
}