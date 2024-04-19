namespace Results.ActionCallables;

internal sealed class ActionCallableWithInput<TIn> : IActionCallable where TIn : notnull {
    private readonly Result<TIn> _result;
    private readonly Action<TIn> _action;
    public ActionCallableWithInput(Result<TIn> result, Action<TIn> action) {
        _result = result;
        _action = action;
    }
    public void Call() {
        if (_result.Failed) return;
        _action(_result.Data);
    }
}