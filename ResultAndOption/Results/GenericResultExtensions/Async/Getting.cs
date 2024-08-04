using ResultAndOption.Results.Getters;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

public static class Getting
{
    /// <summary>
    /// Awaits the result and runs Map
    /// </summary>
    /// <param name="result"></param>
    /// <param name="getter"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> GetAsync<T, TOut>(this Task<Result<T>> result, IResultGetter<TOut> getter)
        where T : notnull where TOut : notnull
    {
        Result<T> awaited = await result;
        return awaited.Get(getter);
    }

    /// <summary>
    /// Awaits the result and runs MapAsync
    /// </summary>
    /// <param name="result"></param>
    /// <param name="getter"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(
        this Task<Result<T>> result,
        IAsyncResultGetter<TOut> getter,
        CancellationToken? token = null)
        where T : notnull where TOut : notnull
    {
        Result<T> awaited = await result;
        return await awaited.GetAsync(getter, token);
    }
}