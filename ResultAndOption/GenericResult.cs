namespace Results;

/// <summary>
///     Represents a result of an operation that can either succeed or fail, carrying either data or an error.
/// </summary>
/// <typeparam name="T">The type of data carried by the result.</typeparam>
public struct Result<T> : IResultWithoutData where T : notnull {
    private readonly Option<T> _data;
    private readonly IError? _error;

    public T Data => _data.Data;

    public bool Failed { get; }

    public bool Succeeded => !Failed;

    public Result() {
        Failed = true;
        _data = Option<T>.None();
        _error = new UnknownError();
    }

    private Result(bool failed, IError? error, Option<T> data) {
        Failed = failed;
        _error = error;
        _data = data;
    }

    public IError Error => !Failed || _error is null
        ? throw new InvalidOperationException()
        : _error;

    /// <summary>
    ///     Creates a successful result with the specified data.
    /// </summary>
    /// <param name="data">The data to carry.</param>
    /// <returns>A successful result carrying the specified data.</returns>
    public static Result<T> Ok(T data) {
        return data is null
            ? throw new InvalidOperationException()
            : new Result<T>(false, null, Option<T>.Some(data));
    }

    /// <summary>
    ///     Creates a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A failed result with the specified error.</returns>
    public static Result<T> Fail(IError error) {
        return new Result<T>(true, error, Option<T>.None());
    }

    /// <summary>
    ///     Creates a result with the specified data and error, where the success or failure depends on the provided data.
    /// </summary>
    /// <param name="data">The data associated with the result.</param>
    /// <param name="error">The error to associate in case of null data</param>
    /// <returns>A result with either the specified data or error. The success or failure depends on the provided data.</returns>
    public static Result<T> Unknown(T? data, IError error) {
        return data is null
            ? Fail(error)
            : Ok(data);
    }

    /// <summary>
    ///     Maps the data of the result using the specified mapper function and wrapps it into a result of the new type.
    /// </summary>
    /// <typeparam name="TResult">The type of data to map to.</typeparam>
    /// <param name="mapper">The mapper function.</param>
    /// <returns>A new result with the mapped data.</returns>
    public Result<TResult> Map<TResult>(Func<T, TResult> mapper) where TResult : notnull {
        return Failed
            ? Result<TResult>.Fail(Error)
            : Result<TResult>.Ok(mapper(Data));
    }

    /// <summary>
    ///     Pipes the data into a function that also returns a result or passes the error to the new result.
    /// </summary>
    /// <typeparam name="TResult">The type of data to map to.</typeparam>
    /// <param name="mapper">The mapper function.</param>
    /// <returns>A new result with the mapped data.</returns>
    public Result<TResult> Map<TResult>(Func<T, Result<TResult>> mapper) where TResult : notnull {
        return Failed
            ? Result<TResult>.Fail(Error)
            : mapper(Data);
    }

    /// <summary>
    ///     Maps the data of the result using the specified function that returns a simple result.
    /// </summary>
    /// <param name="mapper">The function.</param>
    /// <returns>A new result.</returns>
    public Result Map(Func<T, Result> mapper) {
        return Failed
            ? Result.Fail(Error)
            : mapper(Data);
    }

    /// <summary>
    ///     if the result is successfull calls the specified function otherwise returns a simple failed result wrapping the
    ///     current error
    /// </summary>
    /// <param name="function">The function</param>
    /// <returns>A new simple result</returns>
    public Result Map(Func<Result> function) {
        return Failed
            ? Result.Fail(Error)
            : function();
    }


    /// <summary>
    ///     Applies the specified action to the data of the result if it represents a success.
    /// </summary>
    /// <param name="function">The action to apply.</param>
    /// <returns>The same result after applying the action.</returns>
    public Result<T> UseData(Action<T> function) {
        if (!Failed)
            function(Data);
        return this;
    }

    /// <summary>
    ///     Applies the specified function to the data of the result if it represents a success.
    /// </summary>
    /// <param name="function">The function to apply.</param>
    /// <returns>
    ///     The same result after applying the function.
    ///     If the function returns a failed result, a new failed result with the error from the function's result is returned.
    ///     Otherwise, the same result is returned.
    /// </returns>
    public Result<T> UseData(Func<T, Result> function) {
        if (Failed)
            return this;

        var result = function(Data);
        return result.Failed
            ? Fail(result.Error)
            : this;
    }

    /// <summary>
    ///     Converts the result to a simple result without carrying any data.
    /// </summary>
    /// <returns>A simple result representing the success or failure of the original result.</returns>
    public Result ToSimpleResult() {
        return Failed
            ? Result.Fail(Error)
            : Result.Ok();
    }

    /// <summary>
    ///     Converts the result to a result with a different data type, assuming the original result represents an error.
    /// </summary>
    /// <typeparam name="TResult">The type of data carried by the new result.</typeparam>
    /// <returns>A result with the specified data type if the original result represents a success.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the original result represents a failure.</exception>
    public Result<TResult> ConvertErrorResult<TResult>() where TResult : notnull {
        return Failed
            ? Result<TResult>.Fail(Error)
            : throw new InvalidOperationException(
                "Cannot convert error result when the original result represents a success.");
    }

    /// <summary>
    ///     Executes the specified action if the result represents a failure.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>The same result after executing the action.</returns>
    public Result<T> IfFailed(Action<Result<T>> action) {
        if (Failed)
            action(this);
        return this;
    }

    /// <summary>
    ///     Executes the specified action if the result represents a success.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>The same result after executing the action.</returns>
    public Result<T> IfSucceeded(Action<Result<T>> action) {
        if (!Failed)
            action(this);
        return this;
    }

    /// <summary>
    ///     Returns the internal data in case of success or the replacement value passed in
    /// </summary>
    /// <param name="data">Replacement value to use in case of failed result</param>
    /// <returns></returns>
    public T Or(T data) {
        return Failed ? data : Data;
    }

    public async Task<Result<TOut>> MapAsync<TOut>(Func<T, Task<TOut>> asyncFunction) where TOut : notnull {
        if (Failed) return Result<TOut>.Fail(Error);
        var functionData = await asyncFunction(Data);
        return Result<TOut>.Ok(functionData);
    }

    public async Task<Result<TOut>> MapAsync<TOut>(Func<T, Task<Result<TOut>>> asyncMapper) where TOut : notnull {
        if (Failed) return Result<TOut>.Fail(Error);
        return await asyncMapper(Data);
    }

    public async Task<Result> MapAsync(Func<T, Task<Result>> mapper) {
        if (Failed) return Result.Fail(Error);
        return await mapper(Data);
    }

    public async Task<Result> MapAsync(Func<Task<Result>> mapper) =>
        Failed ? Result.Fail(Error) : await mapper();

    /// <summary>
    /// Wraps the existing error if it is a failed result and the error is of the specified type otherwise returns the
    /// existing result object
    /// </summary>
    /// <param name="errorWrapper">function to wrap the error</param>
    /// <typeparam name="TError">expected error type to wrap</typeparam>
    /// <returns></returns>
    public Result<T> WrapError<TError>(Func<TError, IError> errorWrapper) where TError : IError {
        if (Failed && Error is TError error) {
            return Fail(errorWrapper(error));
        }

        return this;
    }
}