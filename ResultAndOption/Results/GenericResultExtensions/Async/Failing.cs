using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

public static class Failing
{
    /// <summary>
    /// Awaits the result and calls the OnError method
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
    /// Awaits the result and calls the OnError method
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
    /// Awaits the result and calls the OnError method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
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
    /// Awaits the result and runs the action if the result is Failed.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
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
    
    /// <summary>
    /// Awaits the result and calls the OnError method.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="command"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, ICommand command)
        where T : notnull
    {
        Result<T> awaited = await result;
        return awaited.OnError(command);
    }

    /// <summary>
    /// Awaits the result and calls the OnErrorAsync method.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="command"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, IAsyncCommand command, CancellationToken? token = null)
        where T : notnull
    {
        Result<T> awaited = await result;
        return await awaited.OnErrorAsync(command, token);
    }

    /// <summary>
    /// Awaits the result and calls the OnError method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="command"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, ICommand<IError> command)
        where T : notnull
    {
        Result<T> awaited = await result;
        return awaited.OnError(command);
    }

    /// <summary>
    /// Awaits the result and calls the OnErrorAsync method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="command"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> OnErrorAsync<T>(
        this Task<Result<T>> result,
        IAsyncCommand<IError> command,
        CancellationToken? token = null)
        where T : notnull
    {
        Result<T> awaited = await result;
        return await awaited.OnErrorAsync(command, token);
    }
}