namespace Results;

public readonly struct Result : IResult {
    private readonly IError? _error;

    public bool Succeeded { get; }
    public bool Failed => !Succeeded;

    public IError Error => Succeeded
        ? throw new InvalidOperationException()
        : _error ?? new UnknownError();

    private Result(bool failed, IError? error) {
        Succeeded = !failed;
        _error = error;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result" /> struct representing a success.
    /// </summary>
    /// <returns>A new successful result.</returns>
    public static Result Ok() {
        return new Result(false, null);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result" /> struct representing a failure with the specified error.
    /// </summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new failed result with the specified error.</returns>
    public static Result Fail(IError error) {
        return new Result(true, error);
    }

    /// <summary>
    ///     Runs an action if the result represents a failure state and returns the same result.
    /// </summary>
    /// <param name="action">The action to run, accepting the current result as a parameter.</param>
    /// <returns>The same result after running the action.</returns>
    public Result IfFailed(Action<Result> action) {
        if (Failed) {
            action(this);
        }
        return this;
    }

    public Result IfFailed(Action action) {
        if (Failed) {
            action();
        }
        return this;
    }
    /// <summary>
    ///     Maps the result using the specified function.
    /// </summary>
    /// <param name="function">The function to map the result.</param>
    /// <returns>
    ///     A new result produced by the function if the original result represents a success. Otherwise, the original
    ///     result is returned.
    /// </returns>
    public Result Map(Func<Result> function) {
        return Failed ? this : function();
    }

    /// <summary>
    ///     Maps the result using the specified function.
    /// </summary>
    /// <typeparam name="T">The type of data carried by the new result.</typeparam>
    /// <param name="function">The function to map the result.</param>
    /// <returns>
    ///     A new result of type <typeparamref name="T" /> produced by the function if the original result represents a
    ///     success. Otherwise, a failed result with the same error as the original result is returned.
    /// </returns>
    public Result<T> Map<T>(Func<Result<T>> function) where T : notnull {
        return Failed ? Result<T>.Fail(_error!) : function();
    }

    /// <summary>
    ///     Maps the result using the specified function.
    /// </summary>
    /// <typeparam name="T">The type of data carried by the new result.</typeparam>
    /// <param name="function">The function to map the result.</param>
    /// <returns>
    ///     A new result of type <typeparamref name="T" /> produced by the function if the original result represents a
    ///     success. Otherwise, a failed result with the same error as the original result is returned.
    /// </returns>
    public Result<T> Map<T>(Func<T> function) where T : notnull {
        return Failed ? Result<T>.Fail(_error!) : Result<T>.Ok(function());
    }

    public async Task<Result> MapAsync(Func<Task<Result>> mapper) {
        return Failed ? this : await mapper();
    }

    public async Task<Result<T>> MapAsync<T>(Func<Task<Result<T>>> mapper) where T : notnull =>
        Failed ? Result<T>.Fail(Error) : await mapper();

    public Result WrapError<TError>(Func<TError, IError> errorWrapper) where TError : IError {
        if (Failed && Error is TError error) {
            return Fail(errorWrapper(error));
        }

        return this;
    }
}