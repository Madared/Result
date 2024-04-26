using Results.Context.CallableGenerators;
using Results.Context.ContextResults;

namespace Results.Context.ContextCallables;

internal sealed class ResultGetterCallableGenerator<TOut> : ICallableGenerator<TOut> where TOut : notnull
{
    private readonly ResultSubscriber<TOut> _resultSubscriber;

    public ResultGetterCallableGenerator(ResultSubscriber<TOut> resultSubscriber)
    {
        _resultSubscriber = resultSubscriber;
    }

    public ICallable<TOut> Generate()
    {
        return new GetterCallable<TOut>(_resultSubscriber.Result);
    }
}