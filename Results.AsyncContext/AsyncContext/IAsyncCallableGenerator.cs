namespace Results.AsyncContext.AsyncContext;

public interface IAsyncCallableGenerator<T> where T : notnull {
    IAsyncCallable<T> Generate();
}