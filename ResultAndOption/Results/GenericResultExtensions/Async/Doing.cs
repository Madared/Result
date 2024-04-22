using ResultAndOption.Errors;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

public static class Doing {
    public static async Task<Result<T>> DoAsync<T>(Result<T> result, Func<T, Task<Result>> action) where T : notnull {
        if (result.Failed) return result;
        Result actionResult = await action(result.Data);
        return actionResult.Failed ? Result<T>.Fail(actionResult.Error) : result;
    }

    public static async Task<Result<T>> DoAsync<T>(this Result<T> result, Func<Task<Result>> action) where T : notnull {
        if (result.Failed) return result;
        Result actionResult = await action();
        return actionResult.Failed ? Result<T>.Fail(actionResult.Error) : result;
    }

    public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Func<T, Result> mapper) where T : notnull {
        Result<T> originalResult = await result;
        return originalResult.Do(mapper);
    }

    public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Func<T, Task<Result>> action) where T : notnull {
        Result<T> originalResult = await result;
        if (originalResult.Failed) return originalResult;
        var actionResult = await action(originalResult.Data);
        return actionResult.Failed ? Result<T>.Fail(actionResult.Error) : originalResult;
    }

    public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Action<T> function) where T : notnull {
        Result<T> originalResult = await result;
        return originalResult.Do(function);
    }

    public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Func<Result> mapper) where T : notnull {
        Result<T> originalResult = await result;
        return originalResult.Do(mapper);
    }

    public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Func<Task<Result>> mapper) where T : notnull {
        Result<T> originalResult = await result;
        if (originalResult.Failed) return originalResult;
        var mappingResult = await mapper();
        return mappingResult.Failed ? Result<T>.Fail(mappingResult.Error) : originalResult;
    }

    public static async Task<Result> IfFailedAsync(this Task<Result> result, Func<IError, Task> action) {
        var original = await result;
        if (original.Failed) await action(original.Error);
        return original;
    }

    public static async Task<Result<T>> IfFailedAsync<T>(this Task<Result<T>> result, Action action) where T : notnull {
        Result<T> original = await result;
        return original.IfFailed(action);
    }

    public static async Task<Result<T>> IfFailedAsync<T>(this Task<Result<T>> result, Action<IError> action) where T : notnull {
        Result<T> original = await result;
        return original.IfFailed(action);
    }

    public static async Task<Result<T>> IfFailedAsync<T>(this Task<Result<T>> result, Func<Task> action) where T : notnull {
        Result<T> original = await result;
        if (original.Failed) await action();

        return original;
    }

    public static async Task<Result<T>> IfFailedAsync<T>(this Task<Result<T>> result, Func<IError, Task> action) where T : notnull {
        Result<T> original = await result;
        if (original.Failed) await action(original.Error);

        return original;
    }
}