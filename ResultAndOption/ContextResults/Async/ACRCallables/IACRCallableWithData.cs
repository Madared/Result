namespace Results.ContextResults.Async;

public interface IACRCallableWithData<TIn, TOut> : IACRCallable<TOut> where TIn : notnull where TOut : notnull {
    public IACRCallableWithData<TIn, TOut> WithData(TIn data);
}