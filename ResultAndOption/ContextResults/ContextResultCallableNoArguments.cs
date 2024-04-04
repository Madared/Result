namespace Results;

public class ContextResultCallableNoArguments<TOut> : IContextResultCallable<TOut> where TOut : notnull {
    private readonly Func<Result<TOut>> _callable;

    public ContextResultCallableNoArguments(Func<Result<TOut>> callable) {
        _callable = callable;
    }

    public Result<TOut> Call() => _callable();
}