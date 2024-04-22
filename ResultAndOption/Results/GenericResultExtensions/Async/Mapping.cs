namespace ResultAndOption.Results.GenericResultExtensions.Async;

public static class Mapping {
    /// <summary>
    /// If the result failed returns a new Result wrapping the error, otherwise awaits for the asyncFunction and wraps it in a Result.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="asyncFunction"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Result<T> result, Func<T, Task<TOut>> asyncFunction) where T : notnull where TOut : notnull {
        if (result.Failed) return Result<TOut>.Fail(result.Error);
        TOut functionData = await asyncFunction(result.Data);
        return Result<TOut>.Ok(functionData);
    }

    /// <summary>
    /// if The result failed returns a new Result wrapping the error, otherwise awaits the asyncMapper and returns its result
    /// </summary>
    /// <param name="result"></param>
    /// <param name="asyncMapper"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Result<T> result, Func<T, Task<Result<TOut>>> asyncMapper) where T : notnull where TOut : notnull => result.Failed 
        ? Result<TOut>.Fail(result.Error) 
        : await asyncMapper(result.Data);
    
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

        TOut asyncResult = await asyncMapper(originalResult.Data);
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
    ///     Implicitly awaits the original result and if it is successful maps it and unwraps the returned result so there are
    ///     no nested results,
    ///     otherwise returns a failed result without calling the mapping function
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
    ///     Implicitly awaits the original result and returns the mapping function result if the original result is successful,
    ///     otherwise returns a failed result without calling the mapping function
    /// </summary>
    /// <param name="result"></param>
    /// <param name="asyncMapper"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, Func<T, Task<Result<TOut>>> asyncMapper) where T : notnull where TOut : notnull {
        Result<T> originalResult = await result;
        if (originalResult.Failed) return Result<TOut>.Fail(originalResult.Error);

        return await asyncMapper(originalResult.Data);
    }
}