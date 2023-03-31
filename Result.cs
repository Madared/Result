namespace Result;

public class Result<T>
{
    private readonly IError? _error;
    private readonly T? _data;
    private readonly bool _failed;

    public bool Failed
    {
        get
        {
            return _failed;
        }
    }
    public bool Succeeded
    {
        get
        {
            return !_failed;
        }
    }

    public IError Error
    {
        get
        {
            if (!_failed)
            {
                throw new InvalidOperationException("Cannot get Error on successful result");
            }
            if (_error is null)
            {
                throw new InvalidOperationException("Cannot have a null error on a failed result");
            }
            return _error;
        }
    }

    public T Data
    {
        get
        {
            if (_failed)
            {
                throw new InvalidOperationException("Cannot get data on a failed result");
            }
            if (_data is null)
            {
                throw new InvalidOperationException("Cannot have null data on successfull result");
            }
            return _data;
        }
    }

    private Result(T? data, IError? error, bool failed)
    {
        _data = data;
        _error = error;
        _failed = failed;
    }

    public static Result<T> Ok(T data)
    {
        return new Result<T>(data, null, false);
    }

    public static Result<T> Fail(IError error)
    {
        return new Result<T>(null, error, true);
    }
}

public class Result
{
    private readonly IError? _error;
    private readonly bool _failed;

    public bool Failed
    {
        get
        {
            return _failed;
        }
    }
    public bool Succeeded
    {
        get
        {
            return !_failed;
        }
    }

    public IError Error
    {
        get
        {
            if (!_failed)
            {
                throw new InvalidOperationException("Cannot get Error on successful result");
            }
            if (_error is null)
            {
                throw new InvalidOperationException("Cannot have a null error on a failed result");
            }
            return _error;
        }
    }

    private Result(bool failed, IError? error)
    {
        _failed = failed;
        _error = error;
    }

    public static Result Ok()
    {
        return new Result(false, null);
    }

    public static Result Fail(IError error)
    {
        return new Result(true, error);
    }

}
