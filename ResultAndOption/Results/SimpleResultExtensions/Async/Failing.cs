using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.SimpleResultExtensions.Async;

public static class Failing
{
    public static async Task<Result> OnErrorAsync(this Task<Result> result, Func<IError, Task> action)
    {
        Result original = await result;
        if (original.Failed) await action(original.Error);
        return original;
    }

    /// <summary>
    /// Awaits the Result and calls the IfFailed method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async Task<Result> OnErrorAsync(this Task<Result> result, Action action)
    {
        Result original = await result;
        return original.OnError(action);
    }

    /// <summary>
    /// Awaits the result and if its a failure awaits the action
    /// </summary>
    /// <param name="result"></param>
    /// <param name="actionVoid"></param>
    /// <returns>The same result</returns>
    public static async Task<Result> OnErrorAsync(
        this Task<Result> result,
        Func<CancellationToken?, Task> actionVoid,
        CancellationToken? token = null)
    {
        Result original = await result;
        if (original.Failed) await actionVoid(token);
        return original;
    }

    public static async Task<Result> OnErrorAsync(
        this Result result,
        Func<CancellationToken?, Task> action,
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
    public static async Task<Result> OnErrorAsync(
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

    public static async Task<Result> OnErrorAsync(
        this Result result,
        Func<IError, CancellationToken?, Task> action,
        CancellationToken? token = null)
    {
        if (result.Failed)
        {
            await action(result.Error, token);
        }

        return result;
    }


    public static async Task<Result> OnErrorAsync(
        this Result result,
        Func<CancellationToken?, Task<Result>> action,
        CancellationToken? token = null)
    {
        return result.Failed ? await action(token) : result;
    }

    public static async Task<Result> OnErrorAsync(
        this Task<Result> result,
        Func<CancellationToken?, Task<Result>> action,
        CancellationToken? token = null)
    {
        Result originalResult = await result;
        return originalResult.Failed ? await action(token) : originalResult;
    }

    public static async Task<Result> OnErrorAsync(
        this Result result,
        Func<IError, CancellationToken?, Task<Result>> action,
        CancellationToken? token = null)
    {
        return result.Failed ? await action(result.Error, token) : result;
    }

    public static async Task<Result> OnErrorAsync(
        this Task<Result> result,
        Func<IError, CancellationToken?, Task<Result>> action,
        CancellationToken? token = null)
    {
        Result originalResult = await result;
        return originalResult.Failed ? await action(originalResult.Error, token) : originalResult;
    }

    public static async Task<Result> OnErrorAsync(
        this Task<Result> result,
        Action<IError> action)
    {
        Result originalResult = await result;
        if (originalResult.Failed)
        {
            action(originalResult.Error);
        }

        return originalResult;
    }
    
    public static async Task<Result> OnErrorAsync(this Task<Result> result, ICommand command)
    {
        Result awaited = await result;
        return awaited.OnError(command);
    }

    public static async Task<Result> OnErrorAsync(this Task<Result> result, ICommand<IError> command)
    {
        Result awaited = await result;
        return awaited.OnError(command);
    }

    public static async Task<Result> OnErrorAsync(this Task<Result> result, IAsyncCommand command)
    {
        Result awaited = await result;
        return await awaited.OnErrorAsync(command);
    }

    public static async Task<Result> OnErrorAsync(this Task<Result> result, IAsyncCommand<IError> command)
    {
        Result awaited = await result;
        return await awaited.OnErrorAsync(command);
    } 
}