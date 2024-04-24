using ResultAndOption.Results;

namespace Results.AsyncContext.AsyncContext.AsyncCallables;

internal sealed class SimpleAsyncCallable<T> : IAsyncCallable<T> where T : notnull {
    private readonly Func<Task<Result<T>>> _func;
    public SimpleAsyncCallable(Func<Task<Result<T>>> func) {
        _func = func;
    }

    public Task<Result<T>> Call() => _func();
}