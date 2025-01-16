namespace ResultAndOption.Results.Getters;

/// <summary>
/// An asynchronous result getter
/// </summary>
/// <typeparam name="T">The result type</typeparam>
public interface IAsyncResultGetter<T> where T : notnull
{
    /// <summary>
    /// Returns a task of a generic result
    /// </summary>
    /// <param name="token">The cancellation token</param>
    /// <returns></returns>
    Task<Result<T>> Get(CancellationToken? token = null);
}