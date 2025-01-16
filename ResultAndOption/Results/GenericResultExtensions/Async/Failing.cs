using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

/// <summary>
/// Extensions for asynchronously handling failures and errors 
/// </summary>
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
    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, Action<CustomError> action)
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
        Func<CustomError, CancellationToken?, Task> action,
        CancellationToken? token = null) where T : notnull
    {
        Result<T> original = await result;
        if (original.Failed)
        {
            await action(original.CustomError, token);
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
    /// Executes a command if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to run</param>
    /// <param name="token">The cancellation token</param>
    /// <typeparam name="T">The type of the result</typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, IAsyncCommand command, CancellationToken? token = null)
        where T : notnull
    {
        Result<T> awaited = await result;
        return await awaited.OnErrorAsync(command, token);
    }

    /// <summary>
    /// Executes a command if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to run</param>
    /// <typeparam name="T">The result type</typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, ICommand<CustomError> command)
        where T : notnull
    {
        Result<T> awaited = await result;
        return awaited.OnError(command);
    }

    /// <summary>
    /// Executes a command if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to run</param>
    /// <param name="token">The cancellation token</param>
    /// <typeparam name="T">The result type</typeparam>
    /// <returns>The same result</returns>
    public static async Task<Result<T>> OnErrorAsync<T>(
        this Task<Result<T>> result,
        IAsyncCommand<CustomError> command,
        CancellationToken? token = null)
        where T : notnull
    {
        Result<T> awaited = await result;
        return await awaited.OnErrorAsync(command, token);
    }
}