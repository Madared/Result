namespace Results;

public class ContextResultCallableOfNotNull<TIn, TOut> : IContextResultCallableWithInput<TIn, TOut> where TIn : notnull where TOut : notnull {
    private readonly TIn _data;
    private readonly Func<TIn, TOut> _callable;

    public ContextResultCallableOfNotNull(TIn data, Func<TIn, TOut> callable) {
        _callable = callable;
        _data = data;
    }

    public Result<TOut> Call() => _callable(_data).ToResult(new UnknownError());
    public IContextResultCallableWithInput<TIn, TOut> WithInput(TIn data) {
        return new ContextResultCallableOfNotNull<TIn, TOut>(data, _callable);
    }
}