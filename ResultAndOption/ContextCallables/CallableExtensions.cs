namespace Results.ContextResultExtensions;

public static class CallableExtensions {
    public static IContextCallable EmptyCallable() {
        return new NoInputSimpleContextCallable(Nothing.DoNothingResult);
    }

    public static IContextCallable ToCallable(this Action action) {
        return new NoInputSimpleContextCallable(action.WrapInResult());
    }

    public static IContextCallable ToCallable(this Func<Result> action) {
        return new NoInputSimpleContextCallable(action);
    }
}

public static class Nothing {
    public static Result DoNothingResult() {
        return Result.Ok();
    }

    public static void DoNothing() { }
}