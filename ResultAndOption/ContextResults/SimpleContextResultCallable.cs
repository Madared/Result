namespace Results;

public class SimpleContextResultCallable<TOut> : IContextResultCallable<TOut> where TOut : notnull {
    private TOut _data;

    public SimpleContextResultCallable(TOut data) {
        _data = data;
    }

    public Result<TOut> Call() => _data.ToResult(new UnknownError());
}