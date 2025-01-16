namespace ResultAndOption.Results.Commands;

/// <summary>
/// An Asynchronous Result Command with input
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAsyncResultCommand<in T> where T : notnull
{
    /// <summary>
    /// Asynchronous Result Command call with input
    /// </summary>
    /// <param name="data">The data required to perform the action</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>The result of the command</returns>
    public Task<Result> Do(T data, CancellationToken? token = null);
}

/// <summary>
/// An Asynchronous Result Command
/// </summary>
public interface IAsyncResultCommand
{
    /// <summary>
    /// Asynchronous Result Command call
    /// </summary>
    /// <param name="token">The cancellation token</param>
    /// <returns></returns>
    public Task<Result> Do(CancellationToken? token = null);
}