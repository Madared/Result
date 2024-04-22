using ResultAndOption.Results;

namespace Context.ContextCallables;

internal sealed class NoInputSimpleCallable : ICallable {
    private readonly Func<Result> _func;

    public NoInputSimpleCallable(Func<Result> func) {
        _func = func;
    }

    public Result Call() {
        return _func();
    }
}