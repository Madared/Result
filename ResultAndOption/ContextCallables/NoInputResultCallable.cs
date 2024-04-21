namespace Results;

internal sealed class NoInputResultCallable<TOut> : IResultCallable<TOut> where TOut : notnull {
    private readonly Func<Result<TOut>> _func;

    public NoInputResultCallable(Func<Result<TOut>> func) {
        _func = func;
    }

    public Result<TOut> Call() {
        return _func();
    }
}