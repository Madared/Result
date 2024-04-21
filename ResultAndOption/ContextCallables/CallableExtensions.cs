namespace Results.ContextResultExtensions;

public static class CallableExtensions {
    public static IResultCallable EmptyCallable() {
        return new NoInputSimpleResultCallable(Nothing.DoNothingResult);
    }

    public static IResultCallable ToCallable(this Action action) {
        return new NoInputSimpleResultCallable(action.WrapInResult());
    }

    public static IResultCallable ToCallable(this Func<Result> action) {
        return new NoInputSimpleResultCallable(action);
    }
}

public static class Nothing {
    public static Result DoNothingResult() {
        return Result.Ok();
    }

    public static void DoNothing() { }
}