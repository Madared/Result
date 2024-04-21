using Results.ActionCallables;

namespace Results.ContextResultExtensions;

internal sealed class CallableWrapCommand : ICommand {
    private readonly IResultCallable _callable;
    private readonly IActionCallable _undoer;

    public CallableWrapCommand(IResultCallable callable, IActionCallable undoer) {
        _callable = callable;
        _undoer = undoer;
    }

    public Result Call() {
        return _callable.Call();
    }

    public void Undo() {
        _undoer.Call();
    }
}