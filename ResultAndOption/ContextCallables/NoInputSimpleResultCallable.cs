namespace Results;

internal sealed class NoInputSimpleResultCallable : IResultCallable {
    private readonly Func<Result> _func;

    public NoInputSimpleResultCallable(Func<Result> func) {
        _func = func;
    }

    public Result Call() {
        return _func();
    }
}