using ResultAndOption.Results.Getters;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

/// <summary>
/// Extensions for getting results asynchronously
/// </summary>
public static class Getting
{
    /// <summary>
    /// Gets a new result if the current one is successful or a result of the new type with the current error
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="getter">The result getter</param>
    /// <typeparam name="T">The type of the input result</typeparam>
    /// <typeparam name="TOut">The type of the output result</typeparam>
    /// <returns>A Task of the new result</returns>
    public static async Task<Result<TOut>> GetAsync<T, TOut>(this Task<Result<T>> result, IResultGetter<TOut> getter)
        where T : notnull where TOut : notnull
    {
        Result<T> awaited = await result;
        return awaited.Get(getter);
    }

    /// <summary>
    /// Gets a new result if the current one is successful or a result of the new type with the current error
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="getter">The result getter</param>
    /// <param name="token">The cancellation token</param>
    /// <typeparam name="T">The type of the input result</typeparam>
    /// <typeparam name="TOut">The type of the output result</typeparam>
    /// <returns>A task of the new result</returns>
    public static async Task<Result<TOut>> GetAsync<T, TOut>(
        this Task<Result<T>> result,
        IAsyncResultGetter<TOut> getter,
        CancellationToken? token = null)
        where T : notnull where TOut : notnull
    {
        Result<T> awaited = await result;
        return await awaited.GetAsync(getter, token);
    }
}