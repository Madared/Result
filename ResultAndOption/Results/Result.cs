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
    public static Result Ok() => new(false, null);

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result" /> struct representing a failure with the specified error.
    /// </summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new failed result with the specified error.</returns>
    public static Result Fail(IError error) => new(true, error);
}