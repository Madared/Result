namespace Results;

public interface IContextCallable<TOut> where TOut : notnull {
    Result<TOut> Call();
}

public interface IContextCallable {
    Result Call();
}