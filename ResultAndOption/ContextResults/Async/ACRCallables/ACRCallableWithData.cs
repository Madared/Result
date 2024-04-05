namespace Results.ContextResults.Async;

public class ACRCallableWithData<TIn, TOut> : IACRCallableWithData<TIn, TOut> where TIn : notnull where TOut : notnull {
    private readonly TIn _data;
    private readonly Func<TIn, Task<Result<TOut>>> _callable;

    public ACRCallableWithData(TIn data, Func<TIn, Task<Result<TOut>>> callable) {
        _data = data;
        _callable = callable;
    }

    public Task<Result<TOut>> Call() => _callable(_data);
    public IACRCallableWithData<TIn, TOut> WithData(TIn data) => new ACRCallableWithData<TIn, TOut>(data, _callable);

    public static ACRCallableWithData<TIn, TOut> Create(TIn data, Func<TIn, Result<TOut>> callable) {
        return new ACRCallableWithData<TIn, TOut>(data, NormalizedCallable);
        Task<Result<TOut>> NormalizedCallable(TIn internalData) => Task.FromResult(callable(internalData));
    }

    public static ACRCallableWithData<TIn, TOut> Create(TIn data, Func<TIn, TOut> callable) {
        return new ACRCallableWithData<TIn, TOut>(data, NormalizedCallable);
        Task<Result<TOut>> NormalizedCallable(TIn internalData) => Task.FromResult(Result<TOut>.Ok(callable(internalData)));
    }

    public static ACRCallableWithData<TIn, TOut> Create(TIn data, Func<TIn, Task<TOut>> callable) {
        return new ACRCallableWithData<TIn, TOut>(data, NormalizedCallable);

        async Task<Result<TOut>> NormalizedCallable(TIn internalData) {
            TOut output = await callable(internalData);
            return Result<TOut>.Ok(output);
        }
    }
    public static ACRCallableWithData<TIn, TOut> Create(TIn data, Func<TIn, Task<Result<TOut>>> callable) => new(data, callable);
}