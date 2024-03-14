namespace Results;

public class Result : IResultWithoutData {
    private readonly IError? _error;

    private Result(bool failed, IError? error) {
        Failed = failed;
        _error = error;
    }

    public bool Succeeded => !Failed;
    public bool Failed { get; }

    public IError Error => !Failed || _error is null
        ? throw new InvalidOperationException()
        : _error;

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
    ///     Runs an action if the result represents a success state and returns the same result.
    /// </summary>
    /// <param name="action">The action to run.</param>
    /// <returns>The same result after running the action.</returns>
    public Result IfSucceeded(Action action) {
        action();
        return this;
    }

    /// <summary>
    ///     Runs an action if the result represents a failure state and returns the same result.
    /// </summary>
    /// <param name="action">The action to run, accepting the current result as a parameter.</param>
    /// <returns>The same result after running the action.</returns>
    public Result IfFailed(Action<Result> action) {
        action(this);
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
}