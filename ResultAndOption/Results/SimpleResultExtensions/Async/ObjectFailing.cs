using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.SimpleResultExtensions.Async;

public static class ObjectFailing
{
    public static async Task<Result> OnErrorAsync(this Task<Result> result, IActionCommand command)
    {
        Result awaited = await result;
        return awaited.OnError(command);
    }

    public static async Task<Result> OnErrorAsync(this Task<Result> result, IActionCommand<IError> command)
    {
        Result awaited = await result;
        return awaited.OnError(command);
    }

    public static async Task<Result> OnErrorAsync(this Task<Result> result, IAsyncActionCommand command)
    {
        Result awaited = await result;
        return await awaited.OnErrorAsync(command);
    }

    public static async Task<Result> OnErrorAsync(this Task<Result> result, IAsyncActionCommand<IError> command)
    {
        Result awaited = await result;
        return await awaited.OnErrorAsync(command);
    } 
}