namespace Results;

public interface IContextResultCallableWithInput<in TIn, TOut> : IContextResultCallable<TOut> where TIn : notnull where TOut : notnull {
    IContextResultCallableWithInput<TIn, TOut> WithInput(TIn data);
}