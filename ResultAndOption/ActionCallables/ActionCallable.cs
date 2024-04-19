namespace Results.ActionCallables;

internal sealed class ActionCallable : IActionCallable {
    private readonly Action _action;

    public ActionCallable(Action action) {
        _action = action;
    }

    public void Call() {
        _action();
    }
}