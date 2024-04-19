namespace Results.ContextResultExtensions;

public static class CallableExtensions {
    public static IContextCallable EmptyCallable() => new NoInputSimpleContextCallable(Nothing.DoNothingResult);
    public static IContextCallable ToCallable(this Action action) => new NoInputSimpleContextCallable(action.WrapInResult());
    public static IContextCallable ToCallable(this Func<Result> action) => new NoInputSimpleContextCallable(action);
}

public static class Nothing {
    public static Result DoNothingResult() => Result.Ok();
    public static void DoNothing() { }
}