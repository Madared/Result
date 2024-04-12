namespace Results;

/// <summary>
/// Extensions For asynchronous result types
/// </summary>
public static class TaskResultExtensions {
    /// <summary>
    ///     implicitly awaits the original result and if the result is a success will also implicitly await the
    ///     async function passed in and maps it;
    /// </summary>
    /// <param name="result">The async result to map</param>
    /// <param name="asyncMapper">The async mapping function</param>
    /// <typeparam name="T">The Original result type</typeparam>
    /// <typeparam name="TOut">The mapper output type</typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result,
        Func<T, Task<TOut>> asyncMapper)
        where TOut : notnull
        where T : notnull {
        Result<T> originalResult = await result;
        if (originalResult.Failed) return Result<TOut>.Fail(originalResult.Error);

        var asyncResult = await asyncMapper(originalResult.Data);
        return Result<TOut>.Ok(asyncResult);
    }

    /// <summary>
    ///     Implicitly awaits the original result and then maps as a normal synchronous Map method
    /// </summary>
    /// <param name="result">The async result to map</param>
    /// <param name="mapper">Mapping function</param>
    /// <typeparam name="T">The Original result type</typeparam>
    /// <typeparam name="TOut">The mapper output type</typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, Func<T, TOut> mapper) where T : notnull where TOut : notnull {
        Result<T> originalResult = await result;
        return originalResult.Map(mapper);
    }

    /// <summary>
    /// Implicitly awaits the original result and if it is successful maps it and unwraps the returned result so there are no nested results,
    /// otherwise returns a failed result without calling the mapping function
    /// </summary>
    /// <param name="result">The async result to map</param>
    /// <param name="mapper">Mapping function</param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, Func<T, Result<TOut>> mapper) where T : notnull where TOut : notnull {
        Result<T> originalResult = await result;
        return originalResult.Map(mapper);
    }

    /// <summary>
    /// Implicitly awaits the original result and returns the mapping function result if the original result is successful,
    /// otherwise returns a failed result without calling the mapping function
    /// </summary>
    /// <param name="result"></param>
    /// <param name="asyncMapper"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, Func<T, Task<Result<TOut>>> asyncMapper) where T : notnull where TOut : notnull {
        Result<T> originalResult = await result;
        if (originalResult.Failed) {
            return Result<TOut>.Fail(originalResult.Error);
        }

        return await asyncMapper(originalResult.Data);
    }

    public static async Task<Result<T>> ToResultAsync<T>(this Task<Option<T>> option, IError error) where T : notnull {
        Option<T> data = await option;
        return data.ToResult(error);
    }

    public static async Task<Result> IfFailedAsync(this Task<Result> result, Action action) {
        Result original = await result;
        return original.IfFailed(action);
    }

    public static async Task<Result> IfFailedAsync(this Task<Result> result, Func<Task> action) {
        Result original = await result;
        if (original.Failed) await action();
        return original;
    }

    public static async Task<Result> IfFailedAsync(this Task<Result> result, Action<IError> action) {
        Result original = await result;
        return original.IfFailed(action);
    }

    public static async Task<Result> IfFailedAsync(this Task<Result> result, Func<IError, Task> action) {
        Result original = await result;
        if (original.Failed) {
            await action(original.Error);
        }
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
        if (original.Failed) {
            await action();
        }

        return original;
    }

    public static async Task<Result<T>> IfFailedAsync<T>(this Task<Result<T>> result, Func<IError, Task> action) where T : notnull {
        Result<T> original = await result;
        if (original.Failed) {
            await action(original.Error);
        }

        return original;
    }

    public static async Task<Result<T>> ToResultAsync<T>(this Task<T?> nullable, IError error) where T : notnull {
        T? data = await nullable;
        return data.ToResult(error);
    }

    public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Func<T, Result> mapper) where T : notnull {
        Result<T> originalResult = await result;
        return originalResult.Do(mapper);
    }

    public static async Task<Result> MapAsync<T>(this Task<Result<T>> result, Func<T, Task<Result>> mapper) where T : notnull {
        Result<T> originalResult = await result;
        return originalResult.Failed ? Result.Fail(originalResult.Error) : await mapper(originalResult.Data);
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
        Result mappingResult = await mapper();
        return mappingResult.Failed ? Result<T>.Fail(mappingResult.Error) : originalResult;
    }

    public static async Task<Result> MapAsync(this Task<Result> result, Func<Task<Result>> asyncMapper) {
        Result originalResult = await result;
        return originalResult.Failed ? originalResult : await asyncMapper();
    }

    public static async Task<Result> MapAsync(this Task<Result> result, Func<Result> mapper) {
        Result originalResult = await result;
        return originalResult.Failed ? originalResult : mapper();
    }
}