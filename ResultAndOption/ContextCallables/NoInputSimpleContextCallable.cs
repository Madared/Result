namespace Results;

internal sealed class NoInputSimpleContextCallable : IContextCallable {
    private readonly Func<Result> _func;

    public NoInputSimpleContextCallable(Func<Result> func) {
        _func = func;
    }

    public Result Call() {
        return _func();
    }
}