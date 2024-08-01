using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

public static class ObjectDoing
{
    /// <summary>
    /// Performs a Do operation on a task of a <see cref="Result{T}"/>
    /// </summary>
    /// <param name="result"></param>
    /// <param name="command"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result> DoAsync<T>(this Task<Result<T>> result, IResultCommand<T> command) where T : notnull
    {
        Result<T> original = await result;
        return original.Do(command);
    }

    /// <summary>
    /// Performs a DoAsync operation on a task of a <see cref="Result{T}"/>
    /// </summary>
    /// <param name="result"></param>
    /// <param name="command"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result> DoAsync<T>(this Task<Result<T>> result, IAsyncResultCommand<T> command)
        where T : notnull
    {
        Result<T> original = await result;
        return await original.DoAsync(command);
    }
}