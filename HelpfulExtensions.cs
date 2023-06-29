namespace Results;

public static class HelpfulExtensions
{
    public static TOut? Pipe<TIn, TOut>(this TIn? i, Func<TIn, TOut?> function)
        where TIn : class
        where TOut : class
    {
        if (i is null)
            return null;
        return function(i);
    }
    
    public static Result Pipe(this Result i, Func<Result> action)
    {
        if (i.Failed)
            return i;
        return action();
    }

    public static Result<TIn> ToResult<TIn>(this TIn? i, IError error)
        where TIn : class =>
        Result<TIn>.Unknown(i, error);

    public static Result<TIn> ToResult<TIn>(this TIn i)
        where TIn : class =>
        Result<TIn>.Ok(i);
}
