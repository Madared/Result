namespace Results.CallableGenerators;

public class CallableGenerator<TIn, TOut> : ICallableGenerator<TOut> where TIn : notnull where TOut : notnull {
    private readonly ResultSubscriber<TIn> _subscriber;
    private readonly Func<TIn, Result<TOut>> _func;
    public CallableGenerator(ResultSubscriber<TIn> subscriber, Func<TIn, Result<TOut>> func) {
        _subscriber = subscriber;
        _func = func;
    }

    public IContextCallable<TOut> Generate() {
        Result<TIn> newResult = _subscriber.Result;
        return newResult.Failed
            ? new NoInputContextCallable<TOut>(() => Result<TOut>.Fail(newResult.Error))
            : new ContextCallable<TIn, TOut>(newResult.Data, _func);
    }
}