namespace Results;

public interface SimpleCRC<in TIn, TOut> : IContextResultCallable<TOut> where TIn : notnull where TOut : notnull {
    SimpleCRC<TIn, TOut> WithInput(TIn data);
}