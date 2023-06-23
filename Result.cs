namespace Result;

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
