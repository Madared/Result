using ResultAndOption.Results;

namespace Results.AsyncContext.AsyncContext.AsyncCallables;

internal sealed class AsyncCallableOfValueWithInput<TIn, TOut> : IAsyncCallable<TOut>
    where TIn : notnull where TOut : notnull {
    private readonly Func<TIn, Task<TOut>> _func;
    private readonly TIn _data;

    public AsyncCallableOfValueWithInput(Func<TIn, Task<TOut>> func, TIn data) {
        _func = func;
        _data = data;
    }

    public async Task<Result<TOut>> Call() => Result<TOut>.Ok(await _func(_data));
}