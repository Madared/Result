namespace ResultAndOption.Results.Commands;

/// <summary>
/// A synchronous Result Command
/// </summary>
public interface ISimpleMapper
{
    /// <summary>
    /// Result Command call
    /// </summary>
    /// <returns></returns>
    public Result Map();
}

/// <summary>
/// A synchronous Result Command with input
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISimpleMapper<in T> where T : notnull
{
    /// <summary>
    /// Synchronous Result Command call with input
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public Result Map(T data);
}