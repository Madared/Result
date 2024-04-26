using ResultAndOption.Results;

namespace Results.Context.ContextCallables;

internal sealed class NoInputCallable<TOut> : ICallable<TOut> where TOut : notnull
{
    private readonly Func<Result<TOut>> _func;

    public NoInputCallable(Func<Result<TOut>> func)
    {
        _func = func;
    }

    public Result<TOut> Call()
    {
        return _func();
    }
}