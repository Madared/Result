namespace Results;

public class CRCOfResult<TIn, TOut> : SimpleCRC<TIn, TOut> where TIn : notnull where TOut : notnull {
    private readonly TIn _data;
    private readonly Func<TIn, Result<TOut>> _callable;

    public CRCOfResult(TIn data, Func<TIn, Result<TOut>> callable) {
        _data = data;
        _callable = callable;
    }

    public Result<TOut> Call() => _callable(_data);
    public SimpleCRC<TIn, TOut> WithInput(TIn data) {
        return new CRCOfResult<TIn, TOut>(data, _callable);
    }
}