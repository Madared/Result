namespace Result;

public class Result<T>
{
    private readonly IError? _error;
    private readonly T? _data;
    private readonly bool _failed;

    public bool Failed => _failed;
    public bool Succeeded => !_failed;

    public IError Error => !_failed || _error is null
        ? throw new InvalidOperationException()
        : _error;

    public T Data => _failed || _data is null
        ? throw new InvalidOperationException()
        : _data;

    private Result(T? data, IError? error, bool failed)
    {
        _data = data;
        _error = error;
        _failed = failed;
    }

    public static Result<T> Ok(T data) => new Result<T>(data, null, false);
    public static Result<T> Fail(IError error) => new Result<T>(null, error, true);
}

public class Result
{
    private readonly IError? _error;
    private readonly bool _failed;

    public bool Failed => _failed;
    public bool Succeeded => !_failed;
    
    public IError Error => !_failed || _error is null
        ? throw new InvalidOperationException()
        : _error;

    private Result(bool failed, IError? error)
    {
        _failed = failed;
        _error = error;
    }

    public static Result Ok() => new Result(false, null);
    public static Result Fail(IError error) => new Result(true, error);

}
