namespace Results.ContextResultExtensions;

internal sealed class CallableWrapCommand : ICommand {
    private readonly IContextCallable _callable;
    private readonly IContextCallable _undoer;
    
    public CallableWrapCommand(IContextCallable callable, IContextCallable undoer) {
        _callable = callable;
        _undoer = undoer;
    }
    public Result Call() => _callable.Call();
    public Result Undo() => _undoer.Call();
}