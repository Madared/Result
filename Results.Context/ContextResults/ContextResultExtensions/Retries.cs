using ResultAndOption;
using ResultAndOption.Errors;

namespace Results.Context.ContextResults.ContextResultExtensions;

public static class Retries {
    public static IContextResult Retry(this IContextResult context, int timesToRetry) {
        if (context.Succeeded) return context;
        int timesRetried = 0;
        while (timesRetried < timesToRetry) {
            IContextResult retried = context.Retry();
            if (retried.Succeeded) return retried;
            timesRetried++;
        }

        return context;
    }

    public static IContextResult<T> Retry<T>(this IContextResult<T> context, int timesToRetry, Func<IError, bool> errorPredicate) where T : notnull {
        IContextResult<T> toRetry = context;
        while (timesToRetry > 0) {
            if (toRetry.Succeeded) return toRetry;
            if (errorPredicate(toRetry.Error) == false) return toRetry;
            IContextResult<T> retried = toRetry.Retry();
            if (retried.Succeeded) return retried;
            toRetry = retried;
            timesToRetry--;
        }

        return context;
    }

    public static IContextResult<T> Retry<T>(this IContextResult<T> context, int timesToRetry) where T : notnull {
        if (context.Succeeded) return context;
        int timesRetried = 0;
        while (timesRetried < timesToRetry) {
            IContextResult<T> retried = context.Retry();
            if (retried.Succeeded) return retried;
            timesRetried++;
        }

        return context;
    }
}