using Results.CallableGenerators;

namespace Results.ContextResultExtensions;

internal class InputCallableGeneratorOfResult<TIn, TOut> : ICallableGenerator<TOut> where TIn : notnull where TOut : notnull {
    private readonly ResultSubscriber<TIn> _subscriber;
    private readonly Func<TIn, Result<TOut>> _func;
    
    public InputCallableGeneratorOfResult(ResultSubscriber<TIn> subscriber, Func<TIn, Result<TOut>> func) {
        _subscriber = subscriber;
        _func = func;
    }
    public IContextCallable<TOut> Generate() {
        return new ContextCallableOfResult<TIn, TOut>(_subscriber.Result, _func);
    }
}