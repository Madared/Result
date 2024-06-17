using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;
using ResultAndOption.Results.Mappers;

namespace ResultAndOption.Results;

public readonly struct Result : IResult
{
    private readonly IError? _error;
    public bool Succeeded { get; }
    public bool Failed => !Succeeded;

    public IError Error => Succeeded
        ? throw new InvalidOperationException()
        : _error ?? new UnknownError();

    private Result(bool failed, IError? error)
    {
        Succeeded = !failed;
        _error = error;
    }

    public Result<TOut> Map<TOut>(IMapper<TOut> mapper) where TOut : notnull => Failed
        ? Result<TOut>.Fail(Error)
        : mapper.Map();

    public Task<Result<TOut>> MapAsync<TOut>(IAsyncMapper<TOut> mapper) where TOut : notnull => Failed
        ? Task.FromResult(Result<TOut>.Fail(Error))
        : mapper.Map();

    public Result Do(ICommand command) => Failed
        ? this
        : command.Do();

    public Task<Result> DoAsync(IAsyncCommand command) => Failed
        ? Task.FromResult(this)
        : command.Do();

    public Result OnError(IActionCommand command)
    {
        if (Failed)
        {
            command.Do();
        }

        return this;
    }

    public Result OnError(IActionCommand<IError> command)
    {
        if (Failed)
        {
            command.Do(Error);
        }

        return this;
    }

    public async Task<Result> OnErrorAsync(IAsyncActionCommand command)
    {
        if (Failed)
        {
            await command.Do();
        }

        return this;
    }

    public async Task<Result> OnErrorAsync(IAsyncActionCommand<IError> command)
    {
        if (Failed)
        {
            await command.Do(Error);
        }

        return this;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result" /> struct representing a success.
    /// </summary>
    /// <returns>A new successful result.</returns>
    public static Result Ok() => new(false, null);

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result" /> struct representing a failure with the specified error.
    /// </summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new failed result with the specified error.</returns>
    public static Result Fail(IError error) => new(true, error);
}