using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.SimpleResultExtensions.Async;

public static class ObjectDoing
{
    public static async Task<Result> DoAsync(this Task<Result> result, ISimpleMapper simpleMapper)
    {
        Result awaited = await result;
        return awaited.Map(simpleMapper);
    }

    public static async Task<Result> DoAsync(this Task<Result> result, IAsyncSimpleMapper simpleMapper)
    {
        Result awaited = await result;
        return await awaited.MapAsync(simpleMapper);
    }

 
}