namespace Results;

internal sealed class CurrentResultActionCallable<TOut> : IContextCallable<TOut> where TOut : notnull {
    private Result<TOut> _result;
    private IContextCallable _callable;

    public CurrentResultActionCallable(Result<TOut> result, IContextCallable callable) {
        _result = result;
        _callable = callable;
    }

    public Result<TOut> Call() {
        Result called = _callable.Call();
        return called.Failed ? Result<TOut>.Fail(called.Error) : _result;
    }
}