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
}

public class Result
{
    private readonly IError _error;
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
}
