using ResultAndOption.Results;

namespace Results.AsyncContext.AsyncContext.AsyncCallables;

internal sealed class AsyncCallableOfValue<T> : IAsyncCallable<T> where T : notnull {
    private readonly Func<Task<T>> _func;

    public AsyncCallableOfValue(Func<Task<T>> func) {
        _func = func;
    }

    public async Task<Result<T>> Call() => Result<T>.Ok(await _func());
}