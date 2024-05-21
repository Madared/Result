using ResultAndOption.Results;
using Results.AsyncContext.AsyncContext.AsyncCallables;
using Results.Context.ContextResults;

namespace Results.AsyncContext.AsyncContext.AsyncCallableGenerators;

internal sealed class InputAsyncCallableGeneratorOfResult<TIn, TOut> : IAsyncCallableGenerator<TOut>
    where TOut : notnull where TIn : notnull
{
    private readonly Func<TIn, Task<Result<TOut>>> _func;
    private readonly ResultSubscriber<TIn> _subscriber;

    public InputAsyncCallableGeneratorOfResult(Func<TIn, Task<Result<TOut>>> func, ResultSubscriber<TIn> subscriber)
    {
        _func = func;
        _subscriber = subscriber;
    }

    public IAsyncCallable<TOut> Generate()
    {
        Result<TIn> data = _subscriber.Result;
        if (data.Failed)
        {
            return new SimpleAsyncCallable<TOut>(() => Task.FromResult(Result<TOut>.Fail(data.Error)));
        }
        return new AsyncCallableWithInput<TIn, TOut>(_func, data.Data);
    }
}