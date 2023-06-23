namespace Result;

public struct Result<T> where T : class
{
    private readonly IError? _error;
    private readonly bool _failed;
    private readonly T? _data;

    public bool Failed => _failed;
    public bool Succeeded => !_failed;

    public IError Error => !_failed || _error is null
        ? throw new InvalidOperationException()
        : _error;

    public T Data => _failed || _data is null
        ? throw new InvalidOperationException()
        : _data;

    private Result(bool failed, IError? error, T? data)
    {
        _failed = failed;
        _error = error;
        _data = data;
    }

    public static Result<T> Ok(T data) => new Result<T>(false, null, data);
    public static Result<T> Fail(IError error) => new Result<T>(true, error, null);
    public static Result<T> Unknown(T? data, IError error) => data is null
        ? new Result<T>(true, error, null)
        : new Result<T>(false, null, data);

    public Result<TResult> Map<TResult>(Func<T, TResult> mapper) where TResult : class =>
        _failed
        ? Result<TResult>.Fail(_error!)
        : Result<TResult>.Ok(mapper(_data!));

    public Result<TResult> Map<TResult>(Func<T, Result<TResult>> mapper) where TResult : class =>
        _failed
        ? Result<TResult>.Fail(_error!)
        : mapper(_data!);

    public Result MapToSimpleResult(Func<T, Result> mapper) =>
        _failed
        ? Result.Fail(_error!)
        : mapper(_data!);

    public Result<T> Mutate(Action<T> function)
    {
        if (_failed)
            return this;
        
        function(_data!);
        return this;
    }

    public Result<T> Mutate(Func<T, Result> function)
    {
        if (_failed)
            return this;
        
        Result result = function(_data!);
        if (result.Failed)
            return Result<T>.Fail(result.Error);
        
        return this;
    }

    public Result ToSimpleResult() =>
        _failed
        ? Result.Fail(_error!)
        : Result.Ok();

    public Result<TResult> ConvertErrorResult<TResult>() where TResult : class =>
        _failed
        ? Result<TResult>.Fail(_error!)
        : throw new InvalidOperationException();

    public Result<T> AddIfFailed(in List<IError> errors)
    {
        if (_failed)
            errors.Add(_error!);
        return this;
    }

    public Result<T> AddIfSucceeded(in List<T> dataList)
    {
        if (!_failed)
            dataList.Add(_data!);
        return this;
    }

    public Result<T> AddIfSucceededOrFailed(in List<IError> errors, in List<T> dataList)
    {
        if (_failed)
            errors.Add(_error!);
        else
            dataList.Add(_data!);
        return this;
    }

    public Result<T> UseData(Action<T> action)
    {
        if (_failed)
            return this;
        action(_data!);
        return this;
    }
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
