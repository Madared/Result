using ResultAndOption.Errors;

namespace ResultAndOption.Results.SimpleResultExtensions.Async;

/// <summary>
/// Contains methods for async actions on Simple Results or actions on Tasks of Simple Results
/// </summary>
public static class Doing
{
    public delegate Task AsyncActionVoid(CancellationToken? token = null);

    public delegate Task<Result> AsyncAction(CancellationToken? token = null);

    public delegate Task<Result> ErrorAsyncAction(IError error, CancellationToken? token = null);

    public delegate Task ErrorAsyncActionVoid(IError error, CancellationToken? token = null);

    /// <summary>
    /// Awaits the result, if its a failure, awaits the action with its error as a param and returns the result,
    /// otherwise just returns the result 
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <returns>The same result</returns>
    public static async Task<Result> IfFailedAsync(this Task<Result> result, Func<IError, Task> action)
    {
        Result original = await result;
        if (original.Failed) await action(original.Error);
        return original;
    }

    public static async Task<Result> IfFailedAsync(
        this Task<Result> result,
        Func<IError, CancellationToken?, Task> action,
        CancellationToken? token = null)
    {
        Result original = await result;
        if (original.Failed)
        {
            await action(original.Error, token);
        }

        return original;
    }

    /// <summary>
    /// Awaits the Result and calls the IfFailed method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async Task<Result> IfFailedAsync(this Task<Result> result, Action action)
    {
        Result original = await result;
        return original.IfFailed(action);
    }

    /// <summary>
    /// Awaits the result and if its a failure awaits the action
    /// </summary>
    /// <param name="result"></param>
    /// <param name="actionVoid"></param>
    /// <returns>The same result</returns>
    public static async Task<Result> IfFailedAsync(
        this Task<Result> result,
        AsyncActionVoid actionVoid,
        CancellationToken? token = null)
    {
        Result original = await result;
        if (original.Failed) await actionVoid(token);
        return original;
    }

    public static async Task<Result> IfFailedAsync(
        this Result result,
        AsyncActionVoid action,
        CancellationToken? token = null)
    {
        if (result.Failed)
        {
            await action(token);
        }

        return result;
    }

    /// <summary>
    /// Awaits the result and calls the IfFailed method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async Task<Result> IfFailedAsync(
        this Task<Result> result,
        ErrorAsyncActionVoid action,
        CancellationToken? token = null)
    {
        Result original = await result;
        if (original.Failed)
        {
            await action(original.Error, token);
        }

        return original;
    }

    public static async Task<Result> IfFailedAsync(
        this Result result,
        ErrorAsyncActionVoid action,
        CancellationToken? token = null)
    {
        if (result.Failed)
        {
            await action(result.Error, token);
        }

        return result;
    }


    public static async Task<Result> IfFailedAsync(
        this Result result,
        AsyncAction action,
        CancellationToken? token = null)
    {
        if (result.Failed)
        {
            return await action(token);
        }

        return result;
    }

    public static async Task<Result> IfFailedAsync(
        this Task<Result> result,
        AsyncAction action,
        CancellationToken? token = null)
    {
        Result originalResult = await result;
        if (originalResult.Failed)
        {
            return await action(token);
        }

        return originalResult;
    }

    public static async Task<Result> IfFailedAsync(
        this Result result,
        ErrorAsyncAction action,
        CancellationToken? token = null)
    {
        if (result.Failed)
        {
            await action(result.Error, token);
        }

        return result;
    }

    public static async Task<Result> IfFailedAsync(
        this Task<Result> result,
        ErrorAsyncAction action,
        CancellationToken? token = null)
    {
        Result originalResult = await result;
        return await originalResult.IfFailedAsync(action, token);
    }

    /// <summary>
    /// If the result is a failure awaits the specified action and returns its result, otherwise returns the same result
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static async Task<Result> DoAsync(this Result result, Func<Task<Result>> mapper) => result.Failed
        ? result
        : await mapper();
}