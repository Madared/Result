using ResultAndOption.Errors;

namespace ResultAndOption.Results;

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
    public Result IfFailed(Action<IError> action) {
        if (Failed) action(Error);
        return this;
    }

    public Result IfFailed(Action action) {
        if (Failed) action();
        return this;
    }





    public async Task<Result> DoAsync(Func<Task<Result>> mapper) {
        return Failed ? this : await mapper();
    }

    public async Task<Result> MapAsync<T>(Func<Task<Result>> mapper) where T : notnull {
        return Failed ? Fail(Error) : await mapper();
    }

    public Result WrapError<TError>(Func<TError, IError> errorWrapper) where TError : IError {
        if (Failed && Error is TError error) return Fail(errorWrapper(error));

        return this;
    }
}