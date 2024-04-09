namespace Results;

public static class FuncExtensions {
    public static Func<Result> WrapInResult(this Action action) => () => {
        action();
        return Result.Ok();
    };

    public static Func<Result<T>> WrapInResult<T>(this Func<T> function) where T : notnull => () => {
        T data = function();
        return Result<T>.Ok(data);
    };

    public static Func<TIn, Result<TOut>> WrapInResult<TIn, TOut>(this Func<TIn, TOut> function) where TIn : notnull where TOut : notnull =>
        (data) => {
            TOut output = function(data);
            return Result<TOut>.Ok(output);
        };

    public static Func<TIn, Result> WrapInResult<TIn>(this Action<TIn> action) where TIn : notnull => (data) => {
        action(data);
        return Result.Ok();
    };

    public static IContextResult RunAndGetContext(this Func<Result> action) => new StartingContextResult(action(), action);

    public static IContextResult<TOut> RunAndGetContext<TOut>(this Func<Result<TOut>> function) where TOut : notnull => new StartingContextResult<TOut>(function(), function);

    public static IContextResult Retry(this IContextResult context,  int timesToRetry) {
        int timesRetried = 0;
        while (timesRetried < timesToRetry) {
            IContextResult retried = context.Retry();
            if (retried.Succeeded) return retried;
            timesRetried++;
        }

        return context;
    }

    public static IContextResult<T> Retry<T>(this IContextResult<T> context, int timesToRetry) where T : notnull {
        int timesRetried = 0;
        while (timesRetried < timesToRetry) {
            IContextResult<T> retried = context.Retry();
            if (retried.Succeeded) return retried;
            timesRetried++;
        }

        return context;
    }
}