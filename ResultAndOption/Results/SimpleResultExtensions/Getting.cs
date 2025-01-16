using ResultAndOption.Results.Getters;

namespace ResultAndOption.Results.SimpleResultExtensions;

/// <summary>
/// Extension methods for getting new results from simple results
/// </summary>
public static class Getting
{
    /// <summary>
    /// Gets a new result if the current one is in a success state
    /// </summary>
    /// <typeparam name="T">The type of the result</typeparam>
    /// <param name="result">The result to check</param>
    /// <param name="getter">The function to map the result.</param>
    /// <returns>
    /// The result obtained from the getter or the current error wrapped in a new result
    /// </returns>
    public static Result<T> Get<T>(this in Result result, Func<Result<T>> getter) where T : notnull => result.Failed
        ? Result<T>.Fail(result.CustomError)
        : getter();

    /// <summary>
    /// Gets a new result if the current one is in a success state
    /// </summary>
    /// <typeparam name="T">The type of the result</typeparam>
    /// <param name="result">The result to check</param>
    /// <param name="getter">The function to map the result.</param>
    /// <returns>
    /// The result obtained from the getter or the current error wrapped in a new result
    /// </returns>
    public static Result<T> Get<T>(this in Result result, Func<T> getter) where T : notnull => result.Failed
        ? Result<T>.Fail(result.CustomError)
        : Result<T>.Ok(getter());

    /// <summary>
    /// Gets a new result if the current one is in a success state
    /// </summary>
    /// <typeparam name="TOut">The type of the result</typeparam>
    /// <param name="result">The result to check</param>
    /// <param name="getter">The function to map the result.</param>
    /// <returns>
    /// The result obtained from the getter or the current error wrapped in a new result
    /// </returns>
    public static Result<TOut> Get<TOut>(this in Result result, IResultGetter<TOut> getter) where TOut : notnull =>
        result.Failed
            ? Result<TOut>.Fail(result.CustomError)
            : getter.Get();

    /// <summary>
    /// Gets a new result if the current one is in a success state
    /// </summary>
    /// <typeparam name="TOut">The type of the result</typeparam>
    /// <param name="result">The result to check</param>
    /// <param name="getter">The function to map the result.</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>
    /// The result obtained from the getter or the current error wrapped in a new result
    /// </returns>
    public static Task<Result<TOut>> GetAsync<TOut>(
        this in Result result,
        IAsyncResultGetter<TOut> getter,
        CancellationToken? token = null) where TOut : notnull => result.Failed
        ? Task.FromResult(Result<TOut>.Fail(result.CustomError))
        : getter.Get(token);
}