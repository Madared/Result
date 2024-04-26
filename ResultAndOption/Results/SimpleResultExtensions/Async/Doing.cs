using ResultAndOption.Errors;

namespace ResultAndOption.Results.SimpleResultExtensions.Async;

/// <summary>
/// Contains methods for async actions on Simple Results or actions on Tasks of Simple Results
/// </summary>
public static class Doing
{
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
    /// <param name="action"></param>
    /// <returns>The same result</returns>
    public static async Task<Result> IfFailedAsync(this Task<Result> result, Func<Task> action)
    {
        Result original = await result;
        if (original.Failed) await action();
        return original;
    }

    /// <summary>
    /// Awaits the result and calls the IfFailed method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async Task<Result> IfFailedAsync(this Task<Result> result, Action<IError> action)
    {
        Result original = await result;
        return original.IfFailed(action);
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