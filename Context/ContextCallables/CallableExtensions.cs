using ResultAndOption.ContextCallables;
using ResultAndOption.ContextResults.ContextResultExtensions;
using ResultAndOption.Results;

namespace ResultAndOption.ContextResultExtensions;

public static class CallableExtensions {
    public static ICallable EmptyCallable() {
        return new NoInputSimpleCallable(Nothing.DoNothingResult);
    }

    public static ICallable ToCallable(this Action action) {
        return new NoInputSimpleCallable(action.WrapInResult());
    }

    public static ICallable ToCallable(this Func<Result> action) {
        return new NoInputSimpleCallable(action);
    }
}

public static class Nothing {
    public static Result DoNothingResult() {
        return Result.Ok();
    }

    public static void DoNothing() { }
}