namespace Results.ContextResultExtensions;

public static class Running {
   
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

 
}