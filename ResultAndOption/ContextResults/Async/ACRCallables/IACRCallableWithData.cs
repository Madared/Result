namespace Results.ContextResults.Async;

public interface IAsyncContextResultCallableWithData<TIn, TOut> : IAsyncContextResultCallable<TOut> where TIn : notnull where TOut : notnull {
    public IAsyncContextResultCallableWithData<TIn, TOut> WithData(TIn data);
}