using ResultAndOption.Errors;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

/// <summary>
/// Contains all the methods to invoke either Task of action on a result or an action on a Task of result
/// </summary>
public static class Doing {
    /// <summary>
    /// If the result is a failure returns it otherwise awaits the action and returns either the existing result or
    /// a new result with the error of the failed action
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> DoAsync<T>(Result<T> result, Func<T, Task<Result>> action) where T : notnull {
        if (result.Failed) return result;
        Result actionResult = await action(result.Data);
        return actionResult.Failed ? Result<T>.Fail(actionResult.Error) : result;
    }

    /// <summary>
    /// If the result is a failure returns it otherwise awaits the action and returns either the existing result or
    /// a new result with the error of the failed action
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> DoAsync<T>(this Result<T> result, Func<Task<Result>> action) where T : notnull {
        if (result.Failed) return result;
        Result actionResult = await action();
        return actionResult.Failed ? Result<T>.Fail(actionResult.Error) : result;
    }

    /// <summary>
    /// Awaits the result and calls the Do method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapper"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Func<T, Result> mapper) where T : notnull {
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
    public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Func<T, Task<Result>> action) where T : notnull {
        Result<T> originalResult = await result;
        if (originalResult.Failed) return originalResult;
        Result actionResult = await action(originalResult.Data);
        return actionResult.Failed ? Result<T>.Fail(actionResult.Error) : originalResult;
    }

    /// <summary>
    /// Awaits the result and calls the Do method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="function"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Action<T> function) where T : notnull {
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
    public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Func<Result> mapper) where T : notnull {
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
    public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Func<Task<Result>> mapper) where T : notnull {
        Result<T> originalResult = await result;
        if (originalResult.Failed) return originalResult;
        Result mappingResult = await mapper();
        return mappingResult.Failed ? Result<T>.Fail(mappingResult.Error) : originalResult;
    }

    /// <summary>
    /// Awaits the result, if its a failure, awaits the action with its error as a param and returns the result,
    /// otherwise just returns the result 
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <returns>The same result</returns>
    public static async Task<Result> IfFailedAsync(this Task<Result> result, Func<IError, Task> action) {
        Result original = await result;
        if (original.Failed) await action(original.Error);
        return original;
    }

    /// <summary>
    /// Awaits the result and calls the IfFailed method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> IfFailedAsync<T>(this Task<Result<T>> result, Action action) where T : notnull {
        Result<T> original = await result;
        return original.IfFailed(action);
    }

    /// <summary>
    /// Awaits the result and calls the IfFailed method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> IfFailedAsync<T>(this Task<Result<T>> result, Action<IError> action) where T : notnull {
        Result<T> original = await result;
        return original.IfFailed(action);
    }

    /// <summary>
    /// Awaits the result, and if its a failure awaits the specified action
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The same result</returns>
    public static async Task<Result<T>> IfFailedAsync<T>(this Task<Result<T>> result, Func<Task> action) where T : notnull {
        Result<T> original = await result;
        if (original.Failed) await action();

        return original;
    }

    /// <summary>
    /// awaits the result, and if its a failure awaits the specified action with the results error
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The same result</returns>
    public static async Task<Result<T>> IfFailedAsync<T>(this Task<Result<T>> result, Func<IError, Task> action) where T : notnull {
        Result<T> original = await result;
        if (original.Failed) await action(original.Error);

        return original;
    }
}