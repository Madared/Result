using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

public static class ObjectFailing
{
    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, IActionCommand command)
        where T : notnull
    {
        Result<T> awaited = await result;
        return awaited.OnError(command);
    }

    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, IAsyncActionCommand command)
        where T : notnull
    {
        Result<T> awaited = await result;
        return await awaited.OnErrorAsync(command);
    }

    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, IActionCommand<IError> command)
        where T : notnull
    {
        Result<T> awaited = await result;
        return awaited.OnError(command);
    }

    public static async Task<Result<T>> OnErrorAsync<T>(
        this Task<Result<T>> result,
        IAsyncActionCommand<IError> command)
        where T : notnull
    {
        Result<T> awaited = await result;
        return await awaited.OnErrorAsync(command);
    }
}