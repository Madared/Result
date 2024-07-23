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
    private readonly IError? _error;

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

    private Result(bool failed, IError? error, Option<T> data)
    {
        Succeeded = !failed;
        _error = error;
        _data = data;
    }

    /// <summary>
    /// Returns the error from a failed result or throws an exception in a successful result.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public IError Error => Succeeded
        ? throw new InvalidOperationException("Cannot access Error on success Result!")
        : _error ?? new UnknownError();

    /// <summary>
    /// Uses an IMapper to map a successful result data through its transformation by taking the data as the only parameter
    /// or into a new failed result with the existing error.
    /// </summary>
    /// <param name="mapper"></param>
    /// <typeparam name="TOut"></typeparam>
    /// <returns>A new result</returns>
    public Result<TOut> Map<TOut>(IMapper<T, TOut> mapper) where TOut : notnull => Failed
        ? Result<TOut>.Fail(Error)
        : mapper.Map(Data);

    /// <summary>
    /// Uses an IAsyncMapper to map a successful result data through its transformation by taking the data as the only parameter
    /// or into a new failed result with the existing error asynchronously.
    /// </summary>
    /// <param name="mapper"></param>
    /// <typeparam name="TOut"></typeparam>
    /// <returns>A Task of the new result</returns>
    public Task<Result<TOut>> MapAsync<TOut>(IAsyncMapper<T, TOut> mapper) where TOut : notnull => Failed
        ? Task.FromResult(Result<TOut>.Fail(Error))
        : mapper.Map(Data);

    /// <summary>
    /// Uses an IMapper to map a successful result data through its transformation without parameters
    /// or into a new failed result with the existing error.
    /// </summary>
    /// <param name="mapper"></param>
    /// <typeparam name="TOut"></typeparam>
    /// <returns>A new result</returns>
    public Result<TOut> Map<TOut>(IMapper<TOut> mapper) where TOut : notnull => Failed
        ? Result<TOut>.Fail(Error)
        : mapper.Map();

    /// <summary>
    /// Uses an IMapper to map a successful result data through its transformation without parameters
    /// /// or into a new failed result with the existing error.
    /// </summary>
    /// <param name="mapper"></param>
    /// <typeparam name="TOut"></typeparam>
    /// <returns>A Task of the new result</returns>
    public Task<Result<TOut>> MapAsync<TOut>(IAsyncMapper<TOut> mapper) where TOut : notnull => Failed
        ? Task.FromResult(Result<TOut>.Fail(Error))
        : mapper.Map();

    /// <summary>
    /// Executes the action in case the result is in a failed state.
    /// </summary>
    /// <param name="command">The action to execute</param>
    /// <returns>The same result</returns>
    public Result<T> OnError(ICommand command)
    {
        if (Failed)
        {
            command.Do();
        }

        return this;
    }

    /// <summary>
    /// Executes an action asynchronously in case the result is in a failed state.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>A task of the same result.</returns>
    public async Task<Result<T>> OnErrorAsync(IAsyncCommand command)
    {
        if (Failed)
        {
            await command.Do();
        }

        return this;
    }

    /// <summary>
    /// Executes an action in case the result is in a failed state by taking the error as the only parameter.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>The same result.</returns>
    public Result<T> OnError(ICommand<IError> command)
    {
        if (Failed)
        {
            command.Do(Error);
        }

        return this;
    }

    /// <summary>
    /// Executes an action asynchronously in case the result is in a failed state by taking the error as the only parameter.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>A Task of the same result.</returns>
    public async Task<Result<T>> OnErrorAsync(IAsyncCommand<IError> command)
    {
        if (Failed)
        {
            await command.Do(Error);
        }

        return this;
    }

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
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A failed result with the specified error.</returns>
    public static Result<T> Fail(IError error) => new(true, error, Option<T>.None());

    /// <summary>
    ///     Creates a result with the specified data and error, where the success or failure depends on the provided data.
    /// </summary>
    /// <param name="data">The data associated with the result.</param>
    /// <param name="error">The error to associate in case of null data</param>
    /// <returns>A result with either the specified data or error. The success or failure depends on the provided data.</returns>
    public static Result<T> Unknown(T? data, IError error) => data is null
        ? Fail(error)
        : Ok(data);
}