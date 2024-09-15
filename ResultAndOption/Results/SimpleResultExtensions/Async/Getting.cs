namespace ResultAndOption.Results.SimpleResultExtensions.Async;

/// <summary>
/// Contains methods for mapping Tasks of Simple results or map them with Async methods
/// </summary>
public static class Getting
{
    public static async Task<Result<T>> GetAsync<T>(
        this Result result,
        Func<CancellationToken?, Task<Result<T>>> mapper,
        CancellationToken? token = null)
        where T : notnull
    {
        return result.Failed
            ? Result<T>.Fail(result.Error)
            : await mapper(token);
    }

    public static async Task<Result<T>> GetAsync<T>(
        this Task<Result> result,
        Func<CancellationToken?, Task<Result<T>>> mapper,
        CancellationToken? token = null)
        where T : notnull
    {
        Result originalResult = await result;
        return originalResult.Failed ? Result<T>.Fail(originalResult.Error) : await mapper(token);
    }

    public static Task<Result<T>> GetAsync<T>(
        this in Result result,
        Func<CancellationToken?, Task<Result<T>>> getter,
        CancellationToken? token = null) where T : notnull => result.Failed
        ? Task.FromResult(Result<T>.Fail(result.Error))
        : getter(token);
}