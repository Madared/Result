namespace ResultAndOption.Results.Commands;

/// <summary>
/// A synchronous void command
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Command call
    /// </summary>
    public void Do();
}

/// <summary>
/// A synchronous void command that takes an input on call
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICommand<in T> where T : notnull
{
    /// <summary>
    /// Command call with input
    /// </summary>
    /// <param name="data">Input</param>
    public void Do(T data);
}