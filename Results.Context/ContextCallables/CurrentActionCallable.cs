using ResultAndOption.Results;

namespace Results.Context.ContextCallables;

internal sealed class CurrentActionCallable<TOut> : ICallable<TOut> where TOut : notnull
{
    private readonly ICallable _callable;
    private readonly Result<TOut> _result;

    public CurrentActionCallable(Result<TOut> result, ICallable callable)
    {
        _result = result;
        _callable = callable;
    }

    public Result<TOut> Call()
    {
        Result called = _callable.Call();
        return called.Failed ? Result<TOut>.Fail(called.Error) : _result;
    }
}