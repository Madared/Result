using ResultAndOption.Results.Getters;
using ResultAndOption.Results.Mappers;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

using Results;

/// <summary>
/// Extension for mapping generic results asynchronously
/// </summary>
public static class Mapping
{
    /// <summary>
    /// Uses an IAsyncMapper to map a successful result data through its transformation by taking the data as the only parameter
    /// or into a new failed result with the existing error asynchronously.
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="mapper">The mapper to run</param>
    /// <param name="token">The cancellation token</param>
    /// <typeparam name="TOut">The type of the output result</typeparam>
    /// <typeparam name="TIn">The type of the input result</typeparam>
    /// <returns>A Task of the new result</returns>
    public static Task<Result<TOut>> MapAsync<TIn, TOut>(
        this Result<TIn> result,
        IAsyncMapper<TIn, TOut> mapper,
        CancellationToken? token = null) where TIn : notnull where TOut : notnull => result.Failed
        ? Task.FromResult(Result<TOut>.Fail(result.CustomError))
        : mapper.Map(result.Data, token);

    /// <summary>
    /// If the result failed returns a new Result wrapping the error, otherwise awaits for the asyncFunction and wraps it in a Result.
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="asyncFunction">The mapping function to run</param>
    /// <param name="token">The cancellation token</param>
    /// <typeparam name="T">The type of the input result</typeparam>
    /// <typeparam name="TOut">The type of the output result</typeparam>
    /// <returns>A task of the new result</returns>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(
        this Result<T> result,
        Func<T, CancellationToken?, Task<TOut>> asyncFunction,
        CancellationToken? token = null) where T : notnull where TOut : notnull
    {
        if (result.Failed) return Result<TOut>.Fail(result.CustomError);
        TOut functionData = await asyncFunction(result.Data, token);
        return Result<TOut>.Ok(functionData);
    }

    /// <summary>
    /// if The result failed returns a new Result wrapping the error, otherwise awaits the asyncMapper and returns its result
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="asyncMapper">The mapper to run</param>
    /// <param name="token">The cancellation token</param>
    /// <typeparam name="T">The type of the input result</typeparam>
    /// <typeparam name="TOut">The type of the output result</typeparam>
    /// <returns>A Task of the new result</returns>
    public static Task<Result<TOut>> MapAsync<T, TOut>(
        this Result<T> result,
        Func<T, CancellationToken?, Task<Result<TOut>>> asyncMapper,
        CancellationToken? token = null) where T : notnull where TOut : notnull
    {
        return result.Failed
            ? Task.FromResult(Result<TOut>.Fail(result.CustomError))
            : asyncMapper(result.Data, token);
    }

    /// <summary>
    ///     implicitly awaits the original result and if the result is a success will also implicitly await the
    ///     async function passed in and maps it;
    /// </summary>
    /// <param name="result">The async result to map</param>
    /// <param name="asyncMapper">The async mapping function</param>
    /// <param name="token">The cancellation token</param>
    /// <typeparam name="T">The Original result type</typeparam>
    /// <typeparam name="TOut">The mapper output type</typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(
        this Task<Result<T>> result,
        Func<T, CancellationToken?, Task<TOut>> asyncMapper,
        CancellationToken? token = null) where TOut : notnull where T : notnull
    {
        Result<T> originalResult = await result;
        if (originalResult.Failed)
        {
            return Result<TOut>.Fail(originalResult.CustomError);
        }

        TOut asyncResult = await asyncMapper(originalResult.Data, token);
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
    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, Func<T, TOut> mapper)
        where T : notnull where TOut : notnull
    {
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
    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, Func<T, Result<TOut>> mapper)
        where T : notnull where TOut : notnull
    {
        Result<T> originalResult = await result;
        return originalResult.Map(mapper);
    }


    /// <summary>
    /// Awaits the result and runs MapAsync
    /// </summary>
    /// <param name="result"></param>
    /// <param name="asyncMapper"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(
        this Task<Result<T>> result,
        Func<T, CancellationToken?, Task<Result<TOut>>> asyncMapper,
        CancellationToken? token = null) where T : notnull where TOut : notnull
    {
        Result<T> original = await result;
        return await original.MapAsync(asyncMapper, token);
    }

    /// <summary>
    /// Awaits the result and runs Map
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapper"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, IMapper<T, TOut> mapper)
        where T : notnull where TOut : notnull
    {
        Result<T> awaited = await result;
        return awaited.Map(mapper);
    }

    /// <summary>
    /// Awaits the result and runs MapAsync.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapper"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(
        this Task<Result<T>> result,
        IAsyncMapper<T, TOut> mapper,
        CancellationToken? token = null)
        where T : notnull where TOut : notnull
    {
        Result<T> awaited = await result;
        return await awaited.MapAsync(mapper, token);
    }

    /// <summary>
    /// Asynchronously checks if the result is a success and returns its data otherwise returns a default value
    /// </summary>
    /// <param name="result">the result to check</param>
    /// <param name="defaultValue">the default value</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<T> OrAsync<T>(this Task<Result<T>> result, T defaultValue) where T : notnull
    {
        Result<T> awaitedResult = await result;
        return awaitedResult.Failed ? defaultValue : awaitedResult.Data;
    }
}