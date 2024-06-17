namespace ResultAndOption.Results.Commands;

/// <summary>
/// An Async void command with input
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAsyncActionCommand<in T> where T : notnull
{
    /// <summary>
    /// Async Command call with input
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public Task Do(T data);
}

/// <summary>
/// An Async void command
/// </summary>
public interface IAsyncActionCommand
{
    /// <summary>
    /// Async Command call
    /// </summary>
    /// <returns></returns>
    public Task Do();
}