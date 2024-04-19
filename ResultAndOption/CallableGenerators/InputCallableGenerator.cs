using Results.CallableGenerators;

namespace Results.ContextResultExtensions;

internal class InputCallableGenerator<TIn, TOut> : ICallableGenerator<TOut> where TIn : notnull where TOut : notnull {
    private readonly ResultSubscriber<TIn> _subscriber;
    private readonly Func<TIn, TOut> _func;

    public InputCallableGenerator(ResultSubscriber<TIn> subscriber, Func<TIn, TOut> func) {
        _subscriber = subscriber;
        _func = func;
    }

    public IContextCallable<TOut> Generate() {
        Result<TIn> result = _subscriber.Result;
        return result.Failed
            ? new NoInputContextCallable<TOut>(() => Result<TOut>.Fail(result.Error))
            : new ContextCallable<TIn, TOut>(result.Data, _func);
    }
}