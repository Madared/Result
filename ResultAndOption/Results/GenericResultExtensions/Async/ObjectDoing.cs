using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

public static class ObjectDoing
{
    public static async Task<Result> DoAsync<T>(this Task<Result<T>> result, ICommand command) where T : notnull
    {
        Result<T> awaited = await result;
        return awaited.Do(command);
    }

    public static async Task<Result> DoAsync<T>(this Task<Result<T>> result, IAsyncCommand command) where T : notnull
    {
        Result<T> awaited = await result;
        return await awaited.DoAsync(command);
    }

    public static async Task<Result> DoAsync<T>(this Task<Result<T>> result, ICommand<T> command) where T : notnull
    {
        Result<T> awaited = await result;
        return awaited.Do(command);
    }

    public static async Task<Result> DoAsync<T>(this Task<Result<T>> result, IAsyncCommand<T> command) where T : notnull
    {
        Result<T> awaited = await result;
        return await awaited.DoAsync(command);
    }
}