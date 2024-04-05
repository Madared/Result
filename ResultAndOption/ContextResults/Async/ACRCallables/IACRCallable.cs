namespace Results.ContextResults.Async;

public interface IAsyncContextResultCallable<T> where T : notnull {
    Task<Result<T>> Call();
}

public class ACRCallableOfResultWithArguments<TIn, TOut> : IAsyncContextResultCallableWithData<TIn, TOut> where TIn : notnull where TOut : notnull {
    private readonly TIn _data;
    private readonly Func<TIn, Result<TOut>> _callable;

    public ACRCallableOfResultWithArguments(TIn data, Func<TIn, Result<TOut>> callable) {
        _data = data;
        _callable = callable;
    }

    public Task<Result<TOut>> Call() => Task.FromResult(_callable(_data));
    public IAsyncContextResultCallableWithData<TIn, TOut> WithData(TIn data) => new ACRCallableOfResultWithArguments<TIn, TOut>(data, _callable);
}

public class ACRCallableOfNotNullWithArguments<TIn, TOut> : IAsyncContextResultCallable<TOut> where TIn : notnull where TOut : notnull {
    private readonly TIn _data;
    private readonly Func<TIn, TOut> _callable;

    public ACRCallableOfNotNullWithArguments(Func<TIn, TOut> callable, TIn data) {
        _callable = callable;
        _data = data;
    }

    public Task<Result<TOut>> Call() {
        TOut output = _callable(_data);
        return Task.FromResult(
            Result<TOut>.Ok(output)
        );
    }
}

public class ACRCallableOfTaskResultWithArguments<TIn, TOut> : IAsyncContextResultCallableWithData<TIn, TOut> where TIn : notnull where TOut : notnull {
    private readonly TIn _data;
    private readonly Func<TIn, Task<Result<TOut>>> _callable;

    public ACRCallableOfTaskResultWithArguments(TIn data, Func<TIn, Task<Result<TOut>>> callable) {
        _data = data;
        _callable = callable;
    }

    public Task<Result<TOut>> Call() => _callable(_data);
    public IAsyncContextResultCallableWithData<TIn, TOut> WithData(TIn data) => new ACRCallableOfTaskResultWithArguments<TIn, TOut>(data, _callable);
}

public class ACRCallableOfTaskNotNullWithArguments<TIn, TOut> : IAsyncContextResultCallableWithData<TIn, TOut> where TIn : notnull where TOut : notnull {
    private readonly TIn _data;
    private readonly Func<TIn, Task<TOut>> _callable;

    public ACRCallableOfTaskNotNullWithArguments(TIn data, Func<TIn, Task<TOut>> callable) {
        _data = data;
        _callable = callable;
    }

    public async Task<Result<TOut>> Call() {
        TOut output = await _callable(_data);
        return Result<TOut>.Ok(output);
    }

    public IAsyncContextResultCallableWithData<TIn, TOut> WithData(TIn data) => new ACRCallableOfTaskNotNullWithArguments<TIn, TOut>(data, _callable);
}

public class ACRCallableOfResult<TOut> : IAsyncContextResultCallable<TOut> where TOut : notnull {
    private readonly Func<Result<TOut>> _callable;
    
    public ACRCallableOfResult(Func<Result<TOut>> callable) {
        _callable = callable;
    }
    public Task<Result<TOut>> Call() => Task.FromResult(_callable());
}

public class ACRCallableOfNotNull<TOut> : IAsyncContextResultCallable<TOut> where TOut : notnull {
    private readonly Func<TOut> _callable;
    
    public ACRCallableOfNotNull(Func<TOut> callable) {
        _callable = callable;
    }

    public Task<Result<TOut>> Call() => Task.FromResult(
        Result<TOut>.Ok(_callable())
    );
}

public class ACRCallableOfResultTask<TOut> : IAsyncContextResultCallable<TOut> where TOut : notnull {
    private readonly Func<Task<Result<TOut>>> _callable;
    
    public ACRCallableOfResultTask(Func<Task<Result<TOut>>> callable) {
        _callable = callable;
    }

    public Task<Result<TOut>> Call() => _callable();
}

public class ACRCallableOfNotNullTask<TOut> : IAsyncContextResultCallable<TOut> where TOut : notnull {
    private readonly Func<Task<TOut>> _callable;
    
    public ACRCallableOfNotNullTask(Func<Task<TOut>> callable) {
        _callable = callable;
    }

    public async Task<Result<TOut>> Call() {
        TOut output = await _callable();
        return Result<TOut>.Ok(output);
    }
}