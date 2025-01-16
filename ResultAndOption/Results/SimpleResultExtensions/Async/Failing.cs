using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.SimpleResultExtensions.Async;

/// <summary>
/// Extensions for handling errors and failed simple results asynchronously
/// </summary>
public static class Failing
{
    /// <summary>
    /// Runs an action in case of a failed result
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="action">The action to run</param>
    /// <returns>The action result</returns>
    public static async Task<Result> OnErrorAsync(this Task<Result> result, Func<CustomError, Task> action)
    {
        Result original = await result;
        if (original.Failed) await action(original.CustomError);
        return original;
    }

    /// <summary>
    /// Awaits the Result and calls the IfFailed method
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="action">The action to run</param>
    /// <returns>The same result</returns>
    public static async Task<Result> OnErrorAsync(this Task<Result> result, Action action)
    {
        Result original = await result;
        return original.OnError(action);
    }

    /// <summary>
    /// Awaits the result and if its a failure awaits the action
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="actionVoid">The action to run</param>
    /// <param name="token">The cancellation token</param>
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

    /// <summary>
    /// Performs an action if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="action">The action to run</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>The same result</returns>
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
    /// Performs an action if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="action">The action to run</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>The same result</returns>
    public static async Task<Result> OnErrorAsync(
        this Task<Result> result,
        Func<CustomError, CancellationToken?, Task> action,
        CancellationToken? token = null)
    {
        Result original = await result;
        if (original.Failed)
        {
            await action(original.CustomError, token);
        }

        return original;
    }

        /// <summary>
    /// Performs an action if the result is in a failed state
        /// </summary>
        /// <param name="result">The result to check</param>
        /// <param name="action">The action to run</param>
        /// <param name="token">The cancellation token</param>
        /// <returns>The same result</returns>
    public static async Task<Result> OnErrorAsync(
        this Result result,
        Func<CustomError, CancellationToken?, Task> action,
        CancellationToken? token = null)
    {
        if (result.Failed)
        {
            await action(result.CustomError, token);
        }

        return result;
    }

    /// <summary>
    /// Performs an action if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="action">The action to run</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>The same result</returns>
    public static async Task<Result> OnErrorAsync(
        this Result result,
        Func<CancellationToken?, Task<Result>> action,
        CancellationToken? token = null)
    {
        return result.Failed ? await action(token) : result;
    }
    
    /// <summary>
    /// Performs an action if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="action">The action to run</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>The same result</returns>
    public static async Task<Result> OnErrorAsync(
        this Task<Result> result,
        Func<CancellationToken?, Task<Result>> action,
        CancellationToken? token = null)
    {
        Result originalResult = await result;
        return originalResult.Failed ? await action(token) : originalResult;
    }

    /// <summary>
    /// Performs an action if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="action">The action to run</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>The same result</returns>
    public static async Task<Result> OnErrorAsync(
        this Result result,
        Func<CustomError, CancellationToken?, Task<Result>> action,
        CancellationToken? token = null)
    {
        return result.Failed ? await action(result.CustomError, token) : result;
    }

    /// <summary>
    /// Performs an action if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="action">The action to run</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>The same result</returns>
    public static async Task<Result> OnErrorAsync(
        this Task<Result> result,
        Func<CustomError, CancellationToken?, Task<Result>> action,
        CancellationToken? token = null)
    {
        Result originalResult = await result;
        return originalResult.Failed ? await action(originalResult.CustomError, token) : originalResult;
    }

    /// <summary>
    /// Performs an action if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="action">The action to run</param>
    /// <returns>The same result</returns>
    public static async Task<Result> OnErrorAsync(
        this Task<Result> result,
        Action<CustomError> action)
    {
        Result originalResult = await result;
        if (originalResult.Failed)
        {
            action(originalResult.CustomError);
        }

        return originalResult;
    }
    
    /// <summary>
    /// Performs an action if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to run</param>
    /// <returns>The same result</returns>
    public static async Task<Result> OnErrorAsync(this Task<Result> result, ICommand command)
    {
        Result awaited = await result;
        return awaited.OnError(command);
    }

    /// <summary>
    /// Performs an action if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to run</param>
    /// <returns>The same result</returns>
    public static async Task<Result> OnErrorAsync(this Task<Result> result, ICommand<CustomError> command)
    {
        Result awaited = await result;
        return awaited.OnError(command);
    }

    /// <summary>
    /// Performs an action if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to run</param>
    /// <returns>The same result</returns>
    public static async Task<Result> OnErrorAsync(this Task<Result> result, IAsyncCommand command)
    {
        Result awaited = await result;
        return await awaited.OnErrorAsync(command);
    }

    /// <summary>
    /// Performs an action if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to run</param>
    /// <returns>The same result</returns>
    public static async Task<Result> OnErrorAsync(this Task<Result> result, IAsyncCommand<CustomError> command)
    {
        Result awaited = await result;
        return await awaited.OnErrorAsync(command);
    } 
}