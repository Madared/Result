namespace Results;

public static class TaskExtensions {
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
    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, Func<T, TOut> mapper)
        where T : notnull
        where TOut : notnull {
        Result<T> originalResult = await result;
        return originalResult.Map(mapper);
    }
}