using ResultAndOption.Results;

namespace Results.Context.ContextCallables;

internal class CallableOf<TIn, TOut> : ICallable<TOut> where TOut : notnull where TIn : notnull {
    private readonly Func<TIn, Result<TOut>> _func;
    private readonly Result<TIn> _result;

    public CallableOf(Result<TIn> result, Func<TIn, Result<TOut>> func) {
        _result = result;
        _func = func;
    }

    public Result<TOut> Call() {
        return _result.Failed ? Result<TOut>.Fail(_result.Error) : _func(_result.Data);
    }
}