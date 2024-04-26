using ResultAndOption.Results;

namespace Results.Context.ContextCallables;

internal sealed class NoOutputCallable<TIn> : ICallable where TIn : notnull
{
    private readonly TIn _data;
    private readonly Func<TIn, Result> _func;

    public NoOutputCallable(TIn data, Func<TIn, Result> func)
    {
        _data = data;
        _func = func;
    }

    public Result Call()
    {
        return _func(_data);
    }
}