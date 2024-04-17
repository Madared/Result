using Results.ContextResultExtensions;

namespace Results.ContextCommands;

public interface IContextCommand {
    Result Do();
    Result Undo();
}

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

public class ContextCommand : IContextCommand {
    private readonly Func<Result> _do;
    private readonly Func<Result> _undo;

    public ContextCommand(Func<Result> doer, Func<Result> undoer) {
        _do = doer;
        _undo = undoer;
    }

    public Result Do() => _do();
    public Result Undo() => _undo();
}

public static class Nothing {
    public static void DoNothing() {}
    public static Result DoNothingResult() => Result.Ok();
}