namespace Results.ContextResultExtensions;

internal class ContextCallableOfResult<TIn, TOut> : IContextCallable<TOut> where TOut : notnull {
    private readonly Result<TIn> _result;
    private readonly Func<TIn, Result<TOut>> _func;
    
    public ContextCallableOfResult(Result<TIn> result, Func<TIn, Result<TOut>> func) {
        _result = result;
        _func = func;
    }
    public Result<TOut> Call() => _result.Failed ? Result<TOut>.Fail(_result.Error) : _func(_result.Data);
}