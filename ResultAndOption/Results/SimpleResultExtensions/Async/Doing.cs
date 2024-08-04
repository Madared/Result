using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.SimpleResultExtensions.Async;

/// <summary>
/// Contains methods for async actions on Simple Results or actions on Tasks of Simple Results
/// </summary>
public static class Doing
{
    /// <summary>
    /// If the result is a failure awaits the specified action and returns its result, otherwise returns the same result
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapper"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Result> DoAsync(
        this Result result,
        Func<CancellationToken?, Task<Result>> mapper,
        CancellationToken? cancellationToken = null) => result.Failed
        ? result
        : await mapper(cancellationToken);

    /// <summary>
    /// Awaits the result and if it's a success awaits the asyncMapper otherwise returns the same result
    /// </summary>
    /// <param name="result"></param>
    /// <param name="asyncMapper"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static async Task<Result> DoAsync(
        this Task<Result> result,
        Func<CancellationToken?, Task<Result>> asyncMapper,
        CancellationToken? token = null)
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
    public static async Task<Result> DoAsync(this Task<Result> result, Func<Result> mapper)
    {
        Result originalResult = await result;
        return originalResult.Do(mapper);
    }
    
    /// <summary>
    /// Awaits the result and runs Do
    /// </summary>
    /// <param name="result"></param>
    /// <param name="resultCommand"></param>
    /// <returns></returns>
    public static async Task<Result> DoAsync(this Task<Result> result, IResultCommand resultCommand)
    {
        Result awaited = await result;
        return awaited.Do(resultCommand);
    }

    /// <summary>
    /// Awaits the result and runs DoAsync
    /// </summary>
    /// <param name="result"></param>
    /// <param name="resultCommand"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static async Task<Result> DoAsync(this Task<Result> result, IAsyncResultCommand resultCommand, CancellationToken? token = null)
    {
        Result awaited = await result;
        return await awaited.DoAsync(resultCommand);
    }
}