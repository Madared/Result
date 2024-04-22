using ResultAndOption.Errors;

namespace ResultAndOption.Results.SimpleResultExtensions.Async;

public static class Doing {
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
}