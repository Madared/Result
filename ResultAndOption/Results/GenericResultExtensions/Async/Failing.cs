using ResultAndOption.Errors;
using ResultAndOption.Results;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

public static class Failing
{
    /// <summary>
    /// Awaits the result and calls the IfFailed method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, Action action) where T : notnull
    {
        Result<T> original = await result;
        return original.OnError(action);
    }

    /// <summary>
    /// Awaits the result and calls the IfFailed method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, Action<IError> action)
        where T : notnull
    {
        Result<T> original = await result;
        return original.OnError(action);
    }
    
    public static async Task<Result<T>> OnErrorAsync<T>(
        this Task<Result<T>> result,
        Func<CancellationToken?, Task> action,
        CancellationToken? token = null) where T : notnull
    {
        Result<T> original = await result;
        if (original.Failed)
        {
            await action(token);
        }

        return original;
    }

    public static async Task<Result<T>> OnErrorAsync<T>(
        this Task<Result<T>> result,
        Func<IError, CancellationToken?, Task> action,
        CancellationToken? token = null) where T : notnull
    {
        Result<T> original = await result;
        if (original.Failed)
        {
            await action(original.Error, token);
        }

        return original;
    }
}