using ResultAndOption.Results.Getters;
namespace ResultAndOption.Results.GenericResultExtensions;

/// <summary>
/// Extension methods for getting new results based on the state of others
/// </summary>
public static class Getting
{
    /// <summary>
    /// Gets a new result if the current one is successful
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="getter">The result getter to call</param>
    /// <typeparam name="TOut">The type of the output result</typeparam>
    /// <typeparam name="TIn">The type of the input result</typeparam>
    /// <returns>A new result</returns>
    public static Result<TOut> Get<TIn, TOut>(this in Result<TIn> result, IResultGetter<TOut> getter) where TIn : notnull where TOut : notnull => result.Failed
        ? Result<TOut>.Fail(result.CustomError)
        : getter.Get();

    /// <summary>
    /// Gets a new result if the current one is successful
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="getter">The result getter to call</param>
    /// <param name="token">The cancellation token</param>
    /// <typeparam name="TOut">The type of the output result</typeparam>
    /// <typeparam name="TIn">The type of the input result</typeparam>
    /// <returns>A Task of the new result</returns>
    public static async Task<Result<TOut>> GetAsync<TIn, TOut>(this Result<TIn> result, IAsyncResultGetter<TOut> getter, CancellationToken? token = null)
        where TIn : notnull where TOut : notnull => result.Failed
        ? Result<TOut>.Fail(result.CustomError)
        : await getter.Get(token);
}