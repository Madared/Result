using ResultAndOption.ContextCallables;
using ResultAndOption.Results;

namespace Context.ContextCallables;

internal sealed class Callable<TIn, TOut> : ICallable<TOut> where TIn : notnull where TOut : notnull {
    private readonly TIn _data;
    private readonly Func<TIn, TOut> _func;

    public Callable(TIn data, Func<TIn, TOut> func) {
        _data = data;
        _func = func;
    }

    public Result<TOut> Call() {
        return Result<TOut>.Ok(_func(_data));
    }
}