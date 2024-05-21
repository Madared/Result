using Results.AsyncContext.AsyncContext.AsyncCallables;

namespace Results.AsyncContext.AsyncContext.AsyncCallableGenerators;

internal sealed class SimpleAsyncCallableGenerator<T> : IAsyncCallableGenerator<T> where T : notnull
{
    private readonly IAsyncCallable<T> _callable;
    public SimpleAsyncCallableGenerator(IAsyncCallable<T> callable)
    {
        _callable = callable;
    }
    public IAsyncCallable<T> Generate() => _callable;
}