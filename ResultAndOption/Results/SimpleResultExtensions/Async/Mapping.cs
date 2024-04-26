namespace ResultAndOption.Results.SimpleResultExtensions.Async;



public delegate Task<Result<TOut>> AsyncMapper<TOut>(CancellationToken? token = null) where TOut : notnull;

/// <summary>
/// Contains methods for mapping Tasks of Simple results or map them with Async methods
/// </summary>
public static class Mapping
{
    /// <summary>
    /// Awaits the result and if its a success awaits the asyncMapper otherwise returns the same result
    /// </summary>
    /// <param name="result"></param>
    /// <param name="asyncMapper"></param>
    /// <returns></returns>
    public static async Task<Result> MapAsync(
        this Task<Result> result,
        Doing.AsyncAction asyncMapper,
        CancellationToken? token)
    {
        Result originalResult = await result;
        return originalResult.Failed ? originalResult : await asyncMapper(token);
    }

    /// <summary>
    /// Awaits the result and calls the Do method
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static async Task<Result> MapAsync(this Task<Result> result, Func<Result> mapper)
    {
        Result originalResult = await result;
        return originalResult.Do(mapper);
    }

    /// <summary>
    /// if the Result is a failure returns a failed result with the same error otherwise awaits the mapper and returns its result
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static async Task<Result> MapAsync(
        this Result result,
        Doing.AsyncAction mapper,
        CancellationToken? token = null)
    {
        return result.Failed
            ? Result.Fail(result.Error)
            : await mapper();
    }

    public static async Task<Result<T>> MapAsync<T>(
        this Result result,
        AsyncMapper<T> mapper,
        CancellationToken? token = null)
        where T : notnull
    {
        return result.Failed
            ? Result<T>.Fail(result.Error)
            : await mapper(token);
    }

    public static async Task<Result<T>> MapAsync<T>(
        this Task<Result> result,
        AsyncMapper<T> mapper,
        CancellationToken? token = null)
        where T : notnull
    {
        Result originalResult = await result;
        return originalResult.Failed ? Result<T>.Fail(originalResult.Error) : await mapper(token);
    }

  
}