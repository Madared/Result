using Results.CallableGenerators;

namespace Results;

internal sealed class CurrentResultActionCallable<TOut> : IContextCallable<TOut> where TOut : notnull {
    private readonly IContextCallable _callable;
    private readonly Result<TOut> _result;

    public CurrentResultActionCallable(Result<TOut> result, IContextCallable callable) {
        _result = result;
        _callable = callable;
    }

    public Result<TOut> Call() {
        var called = _callable.Call();
        return called.Failed ? Result<TOut>.Fail(called.Error) : _result;
    }
}

internal sealed class ResultGetterCallable<TOut> : IContextCallable<TOut> where TOut : notnull {
    private readonly Result<TOut> _result;

    public ResultGetterCallable(Result<TOut> result) {
        _result = result;
    }

    public Result<TOut> Call() {
        return _result;
    }
}

internal sealed class ResultGetterCallableGenerator<TOut> : ICallableGenerator<TOut> where TOut : notnull {
    private readonly ResultSubscriber<TOut> _resultSubscriber;

    public ResultGetterCallableGenerator(ResultSubscriber<TOut> resultSubscriber) {
        _resultSubscriber = resultSubscriber;
    }

    public IContextCallable<TOut> Generate() {
        return new ResultGetterCallable<TOut>(_resultSubscriber.Result);
    }
}