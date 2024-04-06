namespace Results;

public class CRCNoArguments<TOut> : IContextResultCallable<TOut> where TOut : notnull {
    private readonly Func<Result<TOut>> _callable;

    public CRCNoArguments(Func<Result<TOut>> callable) {
        _callable = callable;
    }

    public Result<TOut> Call() => _callable();
}