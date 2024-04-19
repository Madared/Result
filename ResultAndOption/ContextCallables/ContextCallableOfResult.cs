namespace Results.ContextResultExtensions;

internal class ContextCallableOfResult<TIn, TOut> : IContextCallable<TOut> where TOut : notnull {
    private readonly Func<TIn, Result<TOut>> _func;
    private readonly Result<TIn> _result;

    public ContextCallableOfResult(Result<TIn> result, Func<TIn, Result<TOut>> func) {
        _result = result;
        _func = func;
    }

    public Result<TOut> Call() {
        return _result.Failed ? Result<TOut>.Fail(_result.Error) : _func(_result.Data);
    }
}