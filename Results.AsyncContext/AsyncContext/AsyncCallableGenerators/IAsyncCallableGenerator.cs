using Results.AsyncContext.AsyncContext.AsyncCallables;

namespace Results.AsyncContext.AsyncContext.AsyncCallableGenerators;

public interface IAsyncCallableGenerator<T> where T : notnull
{
    IAsyncCallable<T> Generate();
}

internal sealed class AsyncCallableOfValueGenerator<T> : IAsyncCallableGenerator<T> where T : notnull
{
    
    public IAsyncCallable<T> Generate()
    {
        throw new NotImplementedException();
    }
}