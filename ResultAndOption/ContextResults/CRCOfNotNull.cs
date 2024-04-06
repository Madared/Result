namespace Results;

public class CRCOfNotNull<TIn, TOut> : IContextResultCallableWithData<TIn, TOut> where TIn : notnull where TOut : notnull {
    private readonly TIn _data;
    private readonly Func<TIn, TOut> _callable;

    public CRCOfNotNull(TIn data, Func<TIn, TOut> callable) {
        _callable = callable;
        _data = data;
    }

    public Result<TOut> Call() => _callable(_data).ToResult(new UnknownError());
    public IContextResultCallableWithData<TIn, TOut> WithInput(TIn data) {
        return new CRCOfNotNull<TIn, TOut>(data, _callable);
    }
}