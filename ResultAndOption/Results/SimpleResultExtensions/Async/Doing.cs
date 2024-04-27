using ResultAndOption.Errors;

namespace ResultAndOption.Results.SimpleResultExtensions.Async;

/// <summary>
/// Contains methods for async actions on Simple Results or actions on Tasks of Simple Results
/// </summary>
public static class Doing
{
     /// <summary>
    /// Awaits the result, if its a failure, awaits the action with its error as a param and returns the result,
    /// otherwise just returns the result 
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <returns>The same result</returns>
    

    /// <summary>
    /// If the result is a failure awaits the specified action and returns its result, otherwise returns the same result
    /// </summary>
    /// <param name="result"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static async Task<Result> DoAsync(this Result result, Func<Task<Result>> mapper) => result.Failed
        ? result
        : await mapper();
    
    /// <summary>
    /// Awaits the result and if its a success awaits the asyncMapper otherwise returns the same result
    /// </summary>
    /// <param name="result"></param>
    /// <param name="asyncMapper"></param>
    /// <returns></returns>
    public static async Task<Result> DoAsync(
        this Task<Result> result,
        Func<CancellationToken?, Task<Result>> asyncMapper,
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
    public static async Task<Result> DoAsync(this Task<Result> result, Func<Result> mapper)
    {
        Result originalResult = await result;
        return originalResult.Do(mapper);
    }
}