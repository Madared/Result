using ResultAndOption.CallableGenerators;

namespace ResultAndOption.ContextCallables;

internal sealed class CurrentActionCallable<TOut> : ICallable<TOut> where TOut : notnull {
    private readonly ICallable _callable;
    private readonly Result<TOut> _result;

    public CurrentActionCallable(Result<TOut> result, ICallable callable) {
        _result = result;
        _callable = callable;
    }

    public Result<TOut> Call() {
        var called = _callable.Call();
        return called.Failed ? Result<TOut>.Fail(called.Error) : _result;
    }
}

internal sealed class GetterCallable<TOut> : ICallable<TOut> where TOut : notnull {
    private readonly Result<TOut> _result;

    public GetterCallable(Result<TOut> result) {
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

    public ICallable<TOut> Generate() {
        return new GetterCallable<TOut>(_resultSubscriber.Result);
    }
}