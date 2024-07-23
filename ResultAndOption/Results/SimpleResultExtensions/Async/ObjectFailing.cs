using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.SimpleResultExtensions.Async;

public static class ObjectFailing
{
    public static async Task<Result> OnErrorAsync(this Task<Result> result, ICommand command)
    {
        Result awaited = await result;
        return awaited.OnError(command);
    }

    public static async Task<Result> OnErrorAsync(this Task<Result> result, ICommand<IError> command)
    {
        Result awaited = await result;
        return awaited.OnError(command);
    }

    public static async Task<Result> OnErrorAsync(this Task<Result> result, IAsyncCommand command)
    {
        Result awaited = await result;
        return await awaited.OnErrorAsync(command);
    }

    public static async Task<Result> OnErrorAsync(this Task<Result> result, IAsyncCommand<IError> command)
    {
        Result awaited = await result;
        return await awaited.OnErrorAsync(command);
    } 
}