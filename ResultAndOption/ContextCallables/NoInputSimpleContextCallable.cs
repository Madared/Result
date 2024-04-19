namespace Results;

internal sealed class NoInputSimpleContextCallable : IContextCallable {
    private Func<Result> _func;
    public NoInputSimpleContextCallable(Func<Result> func) {
        _func = func;
    }
    public Result Call() => _func();
}