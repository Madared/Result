using Results.CallableGenerators;

namespace Results.ContextResultExtensions;

internal sealed class CallableGeneratorWithSimpleOutput<TIn> : ICallableGenerator where TIn : notnull {
    private readonly Func<TIn, Result> _action;
    private readonly ResultSubscriber<TIn> _subscriber;

    public CallableGeneratorWithSimpleOutput(Func<TIn, Result> action, ResultSubscriber<TIn> subscriber) {
        _action = action;
        _subscriber = subscriber;
    }

    public IContextCallable Generate() {
        Result<TIn> result = _subscriber.Result;
        return result.Failed
            ? new NoInputSimpleContextCallable(() => Result.Fail(result.Error))
            : new NoOutputContextCallable<TIn>(result.Data, _action);
    }
}