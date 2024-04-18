namespace Results.ContextCommands;

public class CallableContextCommand : IContextCommand {
    private readonly IContextCallable _do;
    private readonly IContextCallable _undo;

    public CallableContextCommand(IContextCallable doer, IContextCallable undo) {
        _do = doer;
        _undo = undo;
    }

    public Result Do() => _do.Call();
    public Result Undo() => _undo.Call();
}