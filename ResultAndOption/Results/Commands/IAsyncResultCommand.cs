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
    /// <param name="data"></param>
    /// <returns></returns>
    public Task<Result> Do(T data);
}

/// <summary>
/// An Asynchronous Result Command
/// </summary>
public interface IAsyncResultCommand
{
    /// <summary>
    /// Asynchronous Result Command call
    /// </summary>
    /// <returns></returns>
    public Task<Result> Do();
}