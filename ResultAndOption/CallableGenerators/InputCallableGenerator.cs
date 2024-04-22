using Results.CallableGenerators;

namespace Results.ContextResultExtensions;

internal class InputCallableGenerator<TIn, TOut> : ICallableGenerator<TOut> where TIn : notnull where TOut : notnull {
    private readonly Func<TIn, TOut> _func;
    private readonly ResultSubscriber<TIn> _subscriber;

    public InputCallableGenerator(ResultSubscriber<TIn> subscriber, Func<TIn, TOut> func) {
        _subscriber = subscriber;
        _func = func;
    }

    public ICallable<TOut> Generate() {
        Result<TIn> result = _subscriber.Result;
        return result.Failed
            ? new NoInputCallable<TOut>(() => Result<TOut>.Fail(result.Error))
            : new Callable<TIn, TOut>(result.Data, _func);
    }
}