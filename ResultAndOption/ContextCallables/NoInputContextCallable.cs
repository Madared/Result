namespace Results;

internal sealed class NoInputContextCallable<TOut> : IContextCallable<TOut> where TOut : notnull {
    private Func<Result<TOut>> _func;

    public NoInputContextCallable(Func<Result<TOut>> func) {
        _func = func;
    }

    public Result<TOut> Call() => _func();
}