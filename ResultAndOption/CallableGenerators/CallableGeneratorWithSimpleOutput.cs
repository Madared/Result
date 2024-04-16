namespace Results.CallableGenerators;

public class CallableGeneratorWithSimpleOutput<TIn> : ICallableGenerator where TIn : notnull {
    private readonly ResultSubscriber<TIn> _subscriber;
    private readonly Func<TIn, Result> _func;

    public CallableGeneratorWithSimpleOutput(ResultSubscriber<TIn> subscriber, Func<TIn, Result> func) {
        _subscriber = subscriber;
        _func = func;
    }

    public IContextCallable Generate() {
        Result<TIn> newResult = _subscriber.Result;
        return newResult.Failed
            ? new NoInputSimpleContextCallable(() => Result.Fail(newResult.Error))
            : new NoOutputContextCallable<TIn>(newResult.Data, _func);
    }
}