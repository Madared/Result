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

    public T Data => _data.IsNone()
        ? throw ErrorToExceptionMapper.Map(_error)
        : _data.Data;

    public bool Failed => !Succeeded;
    public bool Succeeded { get; }

    private Result(bool failed, IError? error, Option<T> data)
    {
        Succeeded = !failed;
        _error = error;
        _data = data;
    }

    public IError Error => Succeeded
        ? throw new InvalidOperationException("Cannot access Error on success Result!")
        : _error ?? new UnknownError();

    public Result<TOut> Map<TOut>(IMapper<T, TOut> mapper) where TOut : notnull => Failed
        ? Result<TOut>.Fail(Error)
        : mapper.Map(Data);

    public Task<Result<TOut>> MapAsync<TOut>(IAsyncMapper<T, TOut> mapper) where TOut : notnull => Failed
        ? Task.FromResult(Result<TOut>.Fail(Error))
        : mapper.Map(Data);

    public Result<TOut> Map<TOut>(IMapper<TOut> mapper) where TOut : notnull => Failed
        ? Result<TOut>.Fail(Error)
        : mapper.Map();

    public Task<Result<TOut>> MapAsync<TOut>(IAsyncMapper<TOut> mapper) where TOut : notnull => Failed
        ? Task.FromResult(Result<TOut>.Fail(Error))
        : mapper.Map();

    public Result Do(ICommand command) => Failed
        ? Result.Fail(Error)
        : command.Do();

    public Task<Result> DoAsync(IAsyncCommand command) => Failed
        ? Task.FromResult(Result.Fail(Error))
        : command.Do();

    public Result Do(ICommand<T> command) => Failed
        ? Result.Fail(Error)
        : command.Do(Data);

    public Task<Result> DoAsync(IAsyncCommand<T> command) => Failed
        ? Task.FromResult(Result.Fail(Error))
        : command.Do(Data);

    public Result<T> OnError(IActionCommand command)
    {
        if (Failed)
        {
            command.Do();
        }

        return this;
    }

    public async Task<Result<T>> OnErrorAsync(IAsyncActionCommand command)
    {
        if (Failed)
        {
            await command.Do();
        }

        return this;
    }

    public Result<T> OnError(IActionCommand<IError> command)
    {
        if (Failed)
        {
            command.Do(Error);
        }

        return this;
    }

    public async Task<Result<T>> OnErrorAsync(IAsyncActionCommand<IError> command)
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