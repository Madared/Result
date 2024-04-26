using ResultAndOption.Errors;

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

    /// <summary>
    /// Awaits the result, and if its a failure awaits the specified action
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The same result</returns>
    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, Func<Task> action)
        where T : notnull
    {
        Result<T> original = await result;
        if (original.Failed) await action();

        return original;
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

    /// <summary>
    /// awaits the result, and if its a failure awaits the specified action with the results error
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The same result</returns>
    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, Func<IError, Task> action)
        where T : notnull
    {
        Result<T> original = await result;
        if (original.Failed) await action(original.Error);

        return original;
    }

    public static async Task<Result<T>> OnErrorAsync<T>(
        this Task<Result<T>> result,
        Func<IError, CancellationToken?, Task> action,
        CancellationToken? token = null)
    {
        Result<T> original = await result;
        if (original.Failed)
        {
            await action(original.Error, token);
        }

        return original;
    }
}