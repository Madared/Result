using ResultAndOption.Results.Commands;
using ResultAndOption.Results;
using ResultAndOption.Results.Mappers;

namespace ResultAndOption.Results.SimpleResultExtensions.Async;

public static class ObjectMapping
{
    public static async Task<Result<T>> MapAsync<T>(this Task<Result> result, IMapper<T> mapper) where T : notnull
    {
        Result awaited = await result;
        return awaited.Do(mapper);
    }

    public static async Task<Result<T>> MapAsync<T>(this Task<Result> result, IAsyncMapper<T> mapper) where T : notnull
    {
        Result awaited = await result;
        return await awaited.DoAsync(mapper);
    } 
    

}