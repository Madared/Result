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
    private readonly IError? _error;
    
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
    public IError Error => Succeeded
        ? throw new InvalidOperationException()
        : _error ?? new UnknownError();

    private Result(bool failed, IError? error)
    {
        Succeeded = !failed;
        _error = error;
    }
    
    #region DoMethods
    public Result<TOut> Do<TOut>(IMapper<TOut> mapper) where TOut : notnull => Failed
        ? Result<TOut>.Fail(Error)
        : mapper.Map();

    public Task<Result<TOut>> DoAsync<TOut>(IAsyncMapper<TOut> mapper, CancellationToken? token = null) where TOut : notnull => Failed
        ? Task.FromResult(Result<TOut>.Fail(Error))
        : mapper.Map(token);

    public Result Do(IResultCommand resultCommand) => Failed
        ? this
        : resultCommand.Do();

    public Task<Result> DoAsync(IAsyncResultCommand resultCommand, CancellationToken? token = null) => Failed
        ? Task.FromResult(this)
        : resultCommand.Do(token);

    public Result Do(ICommand command)
    {
        if (Succeeded)
        {
            command.Do();
        }

        return this;
    }

    public async Task<Result> DoAsync(IAsyncCommand command)
    {
        if (Succeeded)
        {
            await command.Do();
        }

        return this;
    } 
    
    /// <summary>
    ///     Maps the result using the specified function.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action">The function to map the result.</param>
    /// <returns>
    ///     A new result produced by the function if the original result represents a success. Otherwise, the original
    ///     result is returned.
    /// </returns>
    public Result Do( Func<Result> action) => Failed
        ? this
        : action();

    /// <summary>
    /// Invokes selected action if the result is a success and returns same result
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action">Action to invoke</param>
    /// <returns></returns>
    public Result Do(Action action)
    {
        if (Failed)
        {
            return this;
        }
        action();
        return Ok();
    }
    
    #endregion

    #region  MapMethods
    /// <summary>
    ///     Maps the result using the specified function.
    /// </summary>
    /// <typeparam name="T">The type of data carried by the new result.</typeparam>
    /// <param name="result"></param>
    /// <param name="function">The function to map the result.</param>
    /// <returns>
    ///     A new result of type <typeparamref name="T" /> produced by the function if the original result represents a
    ///     success. Otherwise, a failed result with the same error as the original result is returned.
    /// </returns>
    public Result<T> Map<T>(Func<Result<T>> function) where T : notnull => Failed
        ? Result<T>.Fail(Error)
        : function();

    /// <summary>
    ///     Maps the result using the specified function.
    /// </summary>
    /// <typeparam name="T">The type of data carried by the new result.</typeparam>
    /// <param name="result"></param>
    /// <param name="function">The function to map the result.</param>
    /// <returns>
    ///     A new result of type <typeparamref name="T" /> produced by the function if the original result represents a
    ///     success. Otherwise, a failed result with the same error as the original result is returned.
    /// </returns>
    public Result<T> Map<T>(Func<T> function) where T : notnull => Failed
        ? Result<T>.Fail(Error)
        : Result<T>.Ok(function());
    #endregion
    
    #region OnErrorMethods
    /// <summary>
    ///     Runs an action if the result represents a failure state and returns the same result.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action">The action to run, accepting the current result as a parameter.</param>
    /// <returns>The same result after running the action.</returns>
    public Result OnError(Action<IError> action)
    {
        if (Failed) action(Error);
        return this;
    }

    /// <summary>
    /// If the result is a failure calls the specified action
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public Result OnError(Action action)
    {
        if (Failed) action();
        return this;
    } 

    public Result OnError(ICommand command)
    {
        if (Failed)
        {
            command.Do();
        }

        return this;
    }

    public Result OnError(ICommand<IError> command)
    {
        if (Failed)
        {
            command.Do(Error);
        }

        return this;
    }

    public async Task<Result> OnErrorAsync(IAsyncCommand command)
    {
        if (Failed)
        {
            await command.Do();
        }

        return this;
    }

    public async Task<Result> OnErrorAsync(IAsyncCommand<IError> command)
    {
        if (Failed)
        {
            await command.Do(Error);
        }

        return this;
    }
    
    #endregion
    
    #region ConversionMethods
    
    /// <summary>
    /// Wraps the results error if its a failure and the type is the same as the specified type, otherwise returns the same result
    /// </summary>
    /// <param name="result"></param>
    /// <param name="errorWrapper"></param>
    /// <typeparam name="TError"></typeparam>
    /// <returns></returns>
    public Result WrapError<TError>(Func<TError, IError> errorWrapper)
        where TError : IError => this is { Failed: true, Error: TError error }
        ? Result.Fail(errorWrapper(error))
        : this;

    #endregion

    #region WrappingMethods
    public Result<T> Wrap<T>(Func<T> mapper) where T : notnull => Failed
        ? Result<T>.Fail(Error)
        : Result<T>.Ok(mapper()); 
    #endregion
    
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