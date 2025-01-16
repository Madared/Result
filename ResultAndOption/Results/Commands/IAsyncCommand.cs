namespace ResultAndOption.Results.Commands;

/// <summary>
/// An Async void command with input
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAsyncCommand<in T> where T : notnull
{
    /// <summary>
    /// Async Command call with input
    /// </summary>
    /// <param name="data">The value for the command</param>
    /// <param name="token">The cancellation token</param>
    /// <returns></returns>
    public Task Do(T data, CancellationToken? token = null);
}

/// <summary>
/// An Async void command
/// </summary>
public interface IAsyncCommand
{
    /// <summary>
    /// Async command call
    /// </summary>
    /// <param name="token">Cancellation token</param>
    /// <returns></returns>
    public Task Do(CancellationToken? token = null);
}