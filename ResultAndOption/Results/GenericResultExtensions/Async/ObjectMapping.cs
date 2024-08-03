using ResultAndOption.Results;
using ResultAndOption.Results.Mappers;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

public static class ObjectMapping
{
    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, IMapper<TOut> mapper)
        where T : notnull where TOut : notnull
    {
        Result<T> awaited = await result;
        return awaited.Map(mapper);
    }

    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, IAsyncMapper<TOut> mapper)
        where T : notnull where TOut : notnull
    {
        Result<T> awaited = await result;
        return await awaited.MapAsync(mapper);
    }

    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, IMapper<T, TOut> mapper)
        where T : notnull where TOut : notnull
    {
        Result<T> awaited = await result;
        return awaited.Map(mapper);
    }

    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, IAsyncMapper<T, TOut> mapper)
        where T : notnull where TOut : notnull
    {
        Result<T> awaited = await result;
        return await awaited.MapAsync(mapper);
    } 
}