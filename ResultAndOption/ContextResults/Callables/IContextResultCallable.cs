namespace Results;

public interface IContextResultCallable<TOut> where TOut : notnull {
    Result<TOut> Call();
}