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
    ///     Returns the internal data in case of success or the replacement value passed in
    /// </summary>
    /// <param name="data">Replacement value to use in case of failed result</param>
    /// <returns></returns>
    public T Or(T data) => Failed ? data : Data;
    
    #region MapMethods
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
        public Task<Result<TOut>> MapAsync<TOut>(IAsyncMapper<T, TOut> mapper, CancellationToken? token = null) where TOut : notnull => Failed
            ? Task.FromResult<Result<TOut>>(Result<TOut>.Fail(Error))
            : mapper.Map(Data, token);
    
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
        public Task<Result<TOut>> MapAsync<TOut>(IAsyncMapper<TOut> mapper, CancellationToken? token = null) where TOut : notnull => Failed
            ? Task.FromResult<Result<TOut>>(Result<TOut>.Fail(Error))
            : mapper.Map(token);
    
        /// <summary>
        /// Maps the data of the result using the specified mapper function and wrapps it into a result of the new type.
        /// </summary>
        /// <typeparam name="TResult">The type of data to map to.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="mapper">The mapper function.</param>
        /// <returns>A new result with the mapped data.</returns>
        public Result<TResult> Map<TResult>(Func<T, TResult> mapper)
            where TResult : notnull => Failed
            ? Result<TResult>.Fail(Error)
            : Result<TResult>.Ok(mapper(Data));
    
        /// <summary>
        ///     Pipes the data into a function that also returns a result or passes the error to the new result.
        /// </summary>
        /// <typeparam name="TResult">The type of data to map to.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="mapper">The mapper function.</param>
        /// <returns>A new result with the mapped data.</returns>
        public Result<TResult> Map<TResult>(Func<T, Result<TResult>> mapper)
            where TResult : notnull => Failed
            ? Result<TResult>.Fail(Error)
            : mapper(Data);
    
    #endregion
    #region DoMethods
    
    /// <summary>
    /// Executes an action if the result is successful otherwise does nothing. Returns the same result.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>The same result.</returns>
    public Result Do(ICommand<T> command)
    {
        if (Succeeded)
        {
            command.Do(Data);
        }
        return this.ToSimpleResult();
    }

    /// <summary>
    /// Executes an action asynchronously if the result is successful otherwise does nothing. Returns the same result.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>A Task of the same result.</returns>
    public async Task<Result> DoAsync(IAsyncCommand<T> command, CancellationToken? token = null)
    {
        if (Succeeded)
        {
            await command.Do(Data, token);
        }

        return this.ToSimpleResult();
    }

    /// <summary>
    /// Performs a command if the current result is a success and returns its result otherwise passes the Error into a new SimpleResult
    /// </summary>
    /// <param name="command">The command to execute</param>
    /// <returns></returns>
    public Result Do(IResultCommand<T> command) => Succeeded
        ? command.Do(Data)
        : Result.Fail(Error);

    /// <summary>
    /// Performs an async command if the current result is a success and returns its result otherwise passes the Error into a new SimpleResult
    /// </summary>
    /// <param name="command">the async command to execute</param>
    /// <returns></returns>
    public async Task<Result> DoAsync(IAsyncResultCommand<T> command, CancellationToken? token = null) => Succeeded
        ? await command.Do(Data, token)
        : Result.Fail(Error);

    /// <summary>
    /// Maps the data of the result using the specified function that returns a simple result.
    /// </summary>
    /// <param name="action">The function.</param>
    /// <returns>A new result.</returns>
    public Result Do(Func<T, Result> action)
    {
        if (Failed) return Result.Fail(Error);
        Result actionResult = action(Data);
        return actionResult.Failed ? Result.Fail(actionResult.Error) : Result.Ok();
    }

    /// <summary>
    ///     if the result is successful calls the specified function otherwise returns a simple failed result wrapping the
    ///     current error
    /// </summary>
    /// <param name="function">The function</param>
    /// <returns>A new simple result</returns>
    public Result Do(Func<Result> function) => Succeeded ? function() : Result.Fail(Error);

    /// <summary>
    ///     Applies the specified action to the data of the result if it represents a success.
    /// </summary>
    /// <param name="function">The action to apply.</param>
    /// <returns>The same result after applying the action.</returns>
    public Result Do(Action<T> function)
    {
        if (Succeeded) function(Data);
        return Result.Fail(Error);
    }
    #endregion
    #region WrapMethods
    /// <summary>
    ///     Wraps the existing error if it is a failed result and the error is of the specified type otherwise returns the
    ///     existing result object
    /// </summary>
    /// <param name="errorWrapper">function to wrap the error</param>
    /// <typeparam name="TError">expected error type to wrap</typeparam>
    /// <returns></returns>
    public Result<T> WrapError<TError>(Func<TError, IError> errorWrapper) where TError : IError =>
        this is { Failed: true, Error: TError error }
            ? Fail(errorWrapper(error))
            : this;

    public Result<TOut> Wrap<TOut>(Func<T, TOut> mapper) where TOut : notnull => Failed
        ? Result<TOut>.Fail(Error)
        : Result<TOut>.Ok(mapper(Data));

    public Result<TOut> Wrap<TOut>(Func<TOut> mapper) where TOut : notnull => Failed
        ? Result<TOut>.Fail(Error)
        : Result<TOut>.Ok(mapper());
    #endregion
    #region OnErrorMethods
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
    public async Task<Result<T>> OnErrorAsync(IAsyncCommand command, CancellationToken? token = null)
    {
        if (Failed)
        {
            await command.Do(token);
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
    public async Task<Result<T>> OnErrorAsync(IAsyncCommand<IError> command, CancellationToken? token = null)
    {
        if (Failed)
        {
            await command.Do(Error, token);
        }

        return this;
    }

    /// <summary>
    ///     Executes the specified action if the result represents a failure.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>The same result after executing the action.</returns>
    public Result<T> OnError(Action<IError> action)
    {
        if (Failed) action(Error);
        return this;
    }

    /// <summary>
    ///     Executes the specified action if the result represents a failure
    /// </summary>
    /// <param name="action">Action to execute</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The same result</returns>
    public Result<T> OnError(Action action)
    {
        if (Failed) action();
        return this;
    }
    #endregion
    #region ConversionMethods
    /// <summary>
    ///     Converts the result to a simple result without carrying any data.
    /// </summary>
    /// <returns>A simple result representing the success or failure of the original result.</returns>
    public Result ToSimpleResult() => Failed
        ? Result.Fail(Error)
        : Result.Ok();

    /// <summary>
    ///     Converts the result to a result with a different data type, assuming the original result represents an error.
    /// </summary>
    /// <typeparam name="TResult">The type of data carried by the new result.</typeparam>
    /// <returns>A result with the specified data type if the original result represents a success.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the original result represents a success.</exception>
    public Result<TResult> ConvertErrorResult<TResult>() where TResult : notnull => Failed
        ? Result<TResult>.Fail(Error)
        : throw new InvalidOperationException(
            "Cannot convert error result when the original result represents a success.");
    #endregion
    
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