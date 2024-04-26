using ResultAndOption.Errors;

namespace ResultAndOption.Results.SimpleResultExtensions.Async;

/// <summary>
/// Contains methods for async actions on Simple Results or actions on Tasks of Simple Results
/// </summary>
public static class Doing
{
    public delegate Task AsyncActionVoid(CancellationToken? token = null);

    public delegate Task<Result> AsyncAction(CancellationToken? token = null);

    public delegate Task<Result> ErrorAsyncAction(IError error, CancellationToken? token = null);

    public delegate Task ErrorAsyncActionVoid(IError error, CancellationToken? token = null);

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
}