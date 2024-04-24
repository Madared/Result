using ResultAndOption.Results;
using ResultAndOption.Results.GenericResultExtensions;

namespace Results.AsyncContext.AsyncContext.AsyncCallables;

public interface IAsyncCallable<T> where T : notnull {
    Task<Result<T>> Call();
}