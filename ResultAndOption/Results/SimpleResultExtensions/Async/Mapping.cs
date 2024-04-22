namespace ResultAndOption.Results.SimpleResultExtensions.Async;

/// <summary>
/// Contains methods for mapping Tasks of Simple results or map them with Async methods
/// </summary>
public static class Mapping {
    /// <summary>
    /// Awaits the result and if its a success awaits the asyncMapper otherwise returns the same result
    /// </summary>
    /// <param name="result"></param>
    /// <param name="asyncMapper"></param>
    /// <returns></returns>
    public static async Task<Result> MapAsync(this Task<Result> result, Func<Task<Result>> asyncMapper) {
        Result originalResult = await result;
        return originalResult.Failed ? originalResult : await asyncMapper();
    }

    /// <summary>
    /// Awaits the result and calls the Do method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static async Task<Result> MapAsync(this Task<Result> result, Func<Result> mapper) {
        Result originalResult = await result;
        return originalResult.Do(mapper);
    }

    /// <summary>
    /// if the Result is a failure returns a failed result with the same error otherwise awaits the mapper and returns its result
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static async Task<Result> MapAsync(this Result result, Func<Task<Result>> mapper) => result.Failed
        ? Result.Fail(result.Error)
        : await mapper();
}