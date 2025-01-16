namespace ResultAndOption.Results.SimpleResultExtensions.Async;

/// <summary>
/// Contains methods for mapping Tasks of Simple results or map them with Async methods
/// </summary>
public static class Getting
{
    /// <summary>
    /// Gets a new result if the current is in a successful state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="getter">The result getter</param>
    /// <param name="token">The cancellation token</param>
    /// <typeparam name="T">The result type</typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> GetAsync<T>(
        this Result result,
        Func<CancellationToken?, Task<Result<T>>> getter,
        CancellationToken? token = null)
        where T : notnull
    {
        return result.Failed
            ? Result<T>.Fail(result.CustomError)
            : await getter(token);
    }

    /// <summary>
    /// Gets a new result if the current is in a successful state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="getter">The result getter</param>
    /// <param name="token">The cancellation token</param>
    /// <typeparam name="T">The result type</typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> GetAsync<T>(
        this Task<Result> result,
        Func<CancellationToken?, Task<Result<T>>> getter,
        CancellationToken? token = null)
        where T : notnull
    {
        Result originalResult = await result;
        return originalResult.Failed ? Result<T>.Fail(originalResult.CustomError) : await getter(token);
    }

    /// <summary>
    /// Gets a new result if the current is in a successful state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="getter">The result getter</param>
    /// <param name="token">The cancellation token</param>
    /// <typeparam name="T">The result type</typeparam>
    /// <returns></returns>
    public static Task<Result<T>> GetAsync<T>(
        this in Result result,
        Func<CancellationToken?, Task<Result<T>>> getter,
        CancellationToken? token = null) where T : notnull => result.Failed
        ? Task.FromResult(Result<T>.Fail(result.CustomError))
        : getter(token);
}