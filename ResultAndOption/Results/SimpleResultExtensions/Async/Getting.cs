using ResultAndOption.Results;
using ResultAndOption.Results.Getters;
using ResultAndOption.Results.Mappers;

namespace ResultAndOption.Results.SimpleResultExtensions.Async;

public delegate Task<Result<TOut>> AsyncMapper<TOut>(CancellationToken? token = null) where TOut : notnull;

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
    
    public static async Task<Result<T>> GetAsync<T>(this Task<Result> result, IGetter<T> getter) where T : notnull
    {
        Result awaited = await result;
        return awaited.Get(getter);
    }

    public static async Task<Result<T>> GetAsync<T>(this Task<Result> result, IAsyncGetter<T> getter, CancellationToken? token = null) where T : notnull
    {
        Result awaited = await result;
        return await awaited.GetAsync(getter, token);
    } 
}