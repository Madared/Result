namespace Results;

public interface IContextResultCallableWithData<in TIn, TOut> : IContextResultCallable<TOut> where TIn : notnull where TOut : notnull {
    IContextResultCallableWithData<TIn, TOut> WithData(TIn data);
}