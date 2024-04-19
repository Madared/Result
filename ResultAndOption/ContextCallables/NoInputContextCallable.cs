namespace Results;

internal sealed class NoInputContextCallable<TOut> : IContextCallable<TOut> where TOut : notnull {
    private readonly Func<Result<TOut>> _func;

    public NoInputContextCallable(Func<Result<TOut>> func) {
        _func = func;
    }

    public Result<TOut> Call() {
        return _func();
    }
}