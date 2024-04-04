namespace Results;

public class ContextResultCallableOfResult<TIn, TOut> : IContextResultCallableWithData<TIn, TOut> where TIn : notnull where TOut : notnull {
    private readonly TIn _data;
    private readonly Func<TIn, Result<TOut>> _callable;

    public ContextResultCallableOfResult(TIn data, Func<TIn, Result<TOut>> callable) {
        _data = data;
        _callable = callable;
    }

    public Result<TOut> Call() => _callable(_data);
    public IContextResultCallableWithData<TIn, TOut> WithData(TIn data) {
        return new ContextResultCallableOfResult<TIn, TOut>(data, _callable);
    }
}