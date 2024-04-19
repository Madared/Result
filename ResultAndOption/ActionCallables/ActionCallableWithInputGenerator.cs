namespace Results.ActionCallables;

internal sealed class ActionCallableWithInputGenerator<TIn> : IActionCallableGenerator where TIn : notnull {
    private readonly ResultSubscriber<TIn> _subscriber;
    private readonly Action<TIn> _action;
    
    public ActionCallableWithInputGenerator(ResultSubscriber<TIn> subscriber, Action<TIn> action) {
        _subscriber = subscriber;
        _action = action;
    }
    public IActionCallable Generate() => new ActionCallableWithInput<TIn>(_subscriber.Result, _action);
}