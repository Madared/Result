using ResultAndOption.Errors;

namespace ResultAndOption.Results.SimpleResultExtensions.Async;

public static class Doing {
    /// <summary>
    ///     Runs an action if the result represents a failure state and returns the same result.
    /// </summary>
    /// <param name="action">The action to run, accepting the current result as a parameter.</param>
    /// <returns>The same result after running the action.</returns>
    public static Result IfFailed(this Result result, Action<IError> action) {
        if (result.Failed) action(result.Error);
        return result;
    }

    public static Result IfFailed(this Result result, Action action) {
        if (result.Failed) action();
        return result;
    }

    public static async Task<Result> IfFailedAsync(this Task<Result> result, Action action) {
        var original = await result;
        return original.IfFailed(action);
    }

    public static async Task<Result> IfFailedAsync(this Task<Result> result, Func<Task> action) {
        var original = await result;
        if (original.Failed) await action();
        return original;
    }

    public static async Task<Result> IfFailedAsync(this Task<Result> result, Action<IError> action) {
        var original = await result;
        return original.IfFailed(action);
    }

    public static async Task<Result> DoAsync(this Result result, Func<Task<Result>> mapper) => result.Failed
        ? result
        : await mapper();
}