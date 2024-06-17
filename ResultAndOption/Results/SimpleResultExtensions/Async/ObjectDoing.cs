using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.SimpleResultExtensions;

public static class ObjectDoing
{
    public static async Task<Result> DoAsync(this Task<Result> result, ICommand command)
    {
        Result awaited = await result;
        return awaited.Do(command);
    }

    public static async Task<Result> DoAsync(this Task<Result> result, IAsyncCommand command)
    {
        Result awaited = await result;
        return await awaited.DoAsync(command);
    }

 
}