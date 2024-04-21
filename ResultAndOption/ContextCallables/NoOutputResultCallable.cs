namespace Results;

internal sealed class NoOutputResultCallable<TIn> : IResultCallable where TIn : notnull {
    private readonly TIn _data;
    private readonly Func<TIn, Result> _func;

    public NoOutputResultCallable(TIn data, Func<TIn, Result> func) {
        _data = data;
        _func = func;
    }

    public Result Call() {
        return _func(_data);
    }
}