namespace Results;

internal sealed class ResultCallable<TIn, TOut> : IResultCallable<TOut> where TIn : notnull where TOut : notnull {
    private readonly TIn _data;
    private readonly Func<TIn, TOut> _func;

    public ResultCallable(TIn data, Func<TIn, TOut> func) {
        _data = data;
        _func = func;
    }

    public Result<TOut> Call() {
        return Result<TOut>.Ok(_func(_data));
    }
}