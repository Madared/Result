using Results.AsyncContext.AsyncContext.AsyncCallables;

namespace Results.AsyncContext.AsyncContext.AsyncCallableGenerators;

public interface IAsyncCallableGenerator<T> where T : notnull
{
    IAsyncCallable<T> Generate();
}