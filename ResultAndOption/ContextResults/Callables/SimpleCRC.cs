namespace Results;

public class SimpleCRC<TOut> : IContextResultCallable<TOut> where TOut : notnull {
    private TOut _data;

    public SimpleCRC(TOut data) {
        _data = data;
    }

    public Result<TOut> Call() => _data.ToResult(new UnknownError());
}