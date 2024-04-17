using Results.CallableGenerators;

namespace Results;

internal sealed class CurrentResultActionCallableGenerator<TOut> : ICallableGenerator<TOut> where TOut : notnull {
    private readonly ResultSubscriber<TOut> _subscriber;
    private readonly IContextCallable _callable;

    public CurrentResultActionCallableGenerator(ResultSubscriber<TOut> subscriber, IContextCallable callable) {
        _subscriber = subscriber;
        _callable = callable;
    }

    public IContextCallable<TOut> Generate() => new CurrentResultActionCallable<TOut>(_subscriber.Result, _callable);
}