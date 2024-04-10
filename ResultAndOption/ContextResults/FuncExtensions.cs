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

    public static IContextResult<TOut> RunAndGetContext<T1, TOut>(this Func<T1, Result<TOut>> function, T1 input1)
        where T1 : notnull
        where TOut : notnull {
        Func<Result<TOut>> generated = () => function(input1);
        return new StartingContextResult<TOut>(generated(), generated);
    }

    public static IContextResult<TOut> RunAndGetContext<T1, T2, TOut>(this Func<T1, T2, Result<TOut>> function, T1 input1, T2 input2)
        where T1 : notnull
        where T2 : notnull
        where TOut : notnull {
        return new StartingContextResult<TOut>(Generated(), Generated);
        Result<TOut> Generated() => function(input1, input2);
    }

    public static IContextResult<TOut> RunAndGetContext<T1, T2, T3, TOut>(this Func<T1, T2, T3, Result<TOut>> function, T1 input1, T2 input2, T3 input3)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where TOut : notnull {
        return new StartingContextResult<TOut>(Generated(), Generated);
        Result<TOut> Generated() => function(input1, input2, input3);
    }

    public static IContextResult<TOut> RunAndGetContext<T1, T2, T3, T4, TOut>(this Func<T1, T2, T3, T4, Result<TOut>> function, T1 input1, T2 input2, T3 input3, T4 input4)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where TOut : notnull {
        return new StartingContextResult<TOut>(Generated(), Generated);
        Result<TOut> Generated() => function(input1, input2, input3, input4);
    }
    
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