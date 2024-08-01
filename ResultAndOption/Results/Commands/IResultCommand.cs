namespace ResultAndOption.Results.Commands;

/// <summary>
/// A synchronous Result Command
/// </summary>
public interface IResultCommand
{
    /// <summary>
    /// Result Command call
    /// </summary>
    /// <returns></returns>
    public Result Do();
}

/// <summary>
/// A synchronous Result Command with input
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResultCommand<in T> where T : notnull
{
    /// <summary>
    /// Synchronous Result Command call with input
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public Result Do(T data);
}