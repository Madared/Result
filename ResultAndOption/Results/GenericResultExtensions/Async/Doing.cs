using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

/// <summary>
/// Contains all the methods to invoke either Task of action on a result or an action on a Task of result
/// </summary>
public static class Doing
{
    /// <summary>
    /// If the result is a failure returns it otherwise awaits the action and returns either the existing result or
    /// a new result with the error of the failed action
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result> DoAsync<T>(
        this Result<T> result,
        Func<T, CancellationToken?, Task<Result>> action,
        CancellationToken? token = null)
        where T : notnull
    {
        if (result.Failed)
        {
            return result.ToSimpleResult();
        }

        return await action(result.Data, token);
    }

    /// <summary>
    /// If the result is a failure returns it otherwise awaits the action and returns either the existing result or
    /// a new result with the error of the failed action
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result> DoAsync<T>(
        this Result<T> result,
        Func<CancellationToken?, Task<Result>> action,
        CancellationToken? token = null) where T : notnull
    {
        if (result.Failed)
        {
            return result.ToSimpleResult();
        }

        return await action(token);
    }

    /// <summary>
    /// Awaits the result and calls the Do method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapper"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result> DoAsync<T>(this Task<Result<T>> result, Func<T, Result> mapper)
        where T : notnull
    {
        Result<T> originalResult = await result;
        return originalResult.Do(mapper);
    }

    /// <summary>
    /// Awaits the result and calls the Do method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result> DoAsync<T>(
        this Task<Result<T>> result,
        Func<T, CancellationToken?, Task<Result>> action,
        CancellationToken? token = null) where T : notnull
    {
        Result<T> original = await result;
        if (original.Failed)
        {
            return original.ToSimpleResult();
        }

        return await action(original.Data, token);
    }

    /// <summary>
    /// Awaits the result and calls the Do method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="function"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result> DoAsync<T>(this Task<Result<T>> result, Action<T> function) where T : notnull
    {
        Result<T> originalResult = await result;
        return originalResult.Do(function);
    }

    /// <summary>
    /// Awaits the result and calls the Do method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapper"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result> DoAsync<T>(this Task<Result<T>> result, Func<Result> mapper) where T : notnull
    {
        Result<T> originalResult = await result;
        return originalResult.Do(mapper);
    }

    /// <summary>
    /// Awaits the result, if its a failure returns it, otherwise calls the action and returns the same result or a new
    /// result with the failed action error
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapper"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result> DoAsync<T>(
        this Task<Result<T>> result,
        Func<CancellationToken?, Task<Result>> action,
        CancellationToken? token = null) where T : notnull
    {
        Result<T> original = await result;
        if (original.Failed)
        {
            return original.ToSimpleResult();
        }

        return await action(token);
    }

    /// <summary>
    /// Performs a Do operation on a task of a <see cref="Result{T}"/>
    /// </summary>
    /// <param name="result"></param>
    /// <param name="command"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result> DoAsync<T>(this Task<Result<T>> result, IResultCommand<T> command) where T : notnull
    {
        Result<T> original = await result;
        return original.Do(command);
    }

    /// <summary>
    /// Performs a DoAsync operation on a task of a <see cref="Result{T}"/>
    /// </summary>
    /// <param name="result"></param>
    /// <param name="command"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result> DoAsync<T>(this Task<Result<T>> result, IAsyncResultCommand<T> command, CancellationToken? token = null)
        where T : notnull
    {
        Result<T> original = await result;
        return await original.DoAsync(command, token);
    }
}