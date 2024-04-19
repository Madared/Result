using Results.ActionCallables;

namespace Results.ContextResultExtensions;

internal sealed class CallableWrapCommand : ICommand {
    private readonly IContextCallable _callable;
    private readonly IActionCallable _undoer;
    
    public CallableWrapCommand(IContextCallable callable, IActionCallable undoer) {
        _callable = callable;
        _undoer = undoer;
    }
    public Result Call() => _callable.Call();
    public void Undo() => _undoer.Call();
}