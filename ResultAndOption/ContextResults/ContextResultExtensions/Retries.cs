namespace Results.ContextResultExtensions;

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
        while (timesToRetry > 0) {
            if (context.Succeeded) return context;
            if (errorPredicate(context.Error) == false) return context;
            IContextResult<T> retried = context.Retry();
            if (retried.Succeeded) return retried;
            context = retried;
            timesToRetry = timesToRetry - 1;
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