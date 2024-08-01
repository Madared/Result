using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.SimpleResultExtensions.Async;

public static class ObjectDoing
{
    public static async Task<Result> DoAsync(this Task<Result> result, IResultCommand resultCommand)
    {
        Result awaited = await result;
        return awaited.Do(resultCommand);
    }

    public static async Task<Result> DoAsync(this Task<Result> result, IAsyncResultCommand resultCommand)
    {
        Result awaited = await result;
        return await awaited.DoAsync(resultCommand);
    }
}