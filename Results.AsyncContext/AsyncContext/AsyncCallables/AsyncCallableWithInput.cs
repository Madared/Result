using ResultAndOption.Results;

namespace Results.AsyncContext.AsyncContext.AsyncCallables;

internal sealed class AsyncCallableWithInput<TIn, TOut> : IAsyncCallable<TOut>
    where TIn : notnull where TOut : notnull
{
    private readonly Func<TIn, Task<Result<TOut>>> _func;
    private readonly TIn _data;

    public AsyncCallableWithInput(Func<TIn, Task<Result<TOut>>> func, TIn data)
    {
        _func = func;
        _data = data;
    }

    public Task<Result<TOut>> Call() => _func(_data);
}