using ResultAndOption;
using Results.Context.ContextResults;

namespace Results.Context.ActionCallables;

internal sealed class ActionCallableWithInputGenerator<TIn> : IActionCallableGenerator where TIn : notnull
{
    private readonly Action<TIn> _action;
    private readonly ResultSubscriber<TIn> _subscriber;

    public ActionCallableWithInputGenerator(ResultSubscriber<TIn> subscriber, Action<TIn> action)
    {
        _subscriber = subscriber;
        _action = action;
    }

    public IActionCallable Generate()
    {
        return new ActionCallableWithInput<TIn>(_subscriber.Result, _action);
    }
}