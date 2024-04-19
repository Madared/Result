namespace Results;

internal sealed class NoOutputContextCallable<TIn> : IContextCallable where TIn : notnull {
    private readonly TIn _data;
    private readonly Func<TIn, Result> _func;
    public NoOutputContextCallable(TIn data, Func<TIn, Result> func) {
        _data = data;
        _func = func;
    }

    public Result Call() => _func(_data);
}
