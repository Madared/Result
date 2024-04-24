using ResultAndOption.Results;

namespace Results.AsyncContext.AsyncContext;

public interface IAsyncCallable<T> where T : notnull {
    Task<Result<T>> Call();
}