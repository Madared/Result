namespace Results;

public class ContextCallable<TIn, TOut> : IContextCallable<TOut> where TIn : notnull where TOut : notnull {
    private readonly TIn _data;
    private readonly Func<TIn, Result<TOut>> _func;

    public ContextCallable(TIn data, Func<TIn, Result<TOut>> func) {
        _data = data;
        _func = func;
    }

    public Result<TOut> Call() => _func(_data);
}