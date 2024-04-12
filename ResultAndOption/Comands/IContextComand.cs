namespace Results.Comands;

public class ContextComand {
    private readonly Func<Result> _action;
    private readonly Action _undo;

    public ContextComand(Func<Result> action, Action undo) {
        _action = action;
        _undo = undo;
    }

    public Result Do() => _action();
    public void Undo() => _undo();

    public static implicit operator ContextComand(Func<Result> action) => new ContextComand(action, NoComand.Nothing);
    public static implicit operator ContextComand(Action action) => new ContextComand(action.WrapInResult(), NoComand.Nothing);
}

public class ContextComand<TIn> where TIn : notnull {
    private readonly Func<TIn, Result> _action;
    private readonly Action<TIn> _undo;

    public ContextComand(Func<TIn, Result> action, Action<TIn> undo) {
        _action = action;
        _undo = undo;
    }

    Result Do(TIn input) => _action(input);
    void Undo(TIn input) => _undo(input);
}

internal static class NoComand {
    public static void Nothing() {}
}