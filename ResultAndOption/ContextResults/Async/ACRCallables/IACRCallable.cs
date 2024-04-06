namespace Results.ContextResults.Async;

public interface IACRCallable<T> where T : notnull {
    Task<Result<T>> Call();
}