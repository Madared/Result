using ResultAndOption.Results;

namespace Results.Context.ContextResults.ContextResultExtensions;

public static class Wrapping
{
    public static Func<Result> WrapInResult(this Action action)
    {
        return () =>
        {
            action();
            return Result.Ok();
        };
    }

    public static Func<Result<T>> WrapInResult<T>(this Func<T> function) where T : notnull
    {
        return () =>
        {
            T data = function();
            return Result<T>.Ok(data);
        };
    }

    public static Func<TIn, Result<TOut>> WrapInResult<TIn, TOut>(this Func<TIn, TOut> function)
        where TIn : notnull where TOut : notnull
    {
        return data =>
        {
            TOut output = function(data);
            return Result<TOut>.Ok(output);
        };
    }

    public static Func<TIn, Result> WrapInResult<TIn>(this Action<TIn> action) where TIn : notnull
    {
        return data =>
        {
            action(data);
            return Result.Ok();
        };
    }
}