using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;
using ResultAndOption.Results;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

public static class ObjectFailing
{
    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, ICommand command)
        where T : notnull
    {
        Result<T> awaited = await result;
        return awaited.OnError(command);
    }

    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, IAsyncCommand command)
        where T : notnull
    {
        Result<T> awaited = await result;
        return await awaited.OnErrorAsync(command);
    }

    public static async Task<Result<T>> OnErrorAsync<T>(this Task<Result<T>> result, ICommand<IError> command)
        where T : notnull
    {
        Result<T> awaited = await result;
        return awaited.OnError(command);
    }

    public static async Task<Result<T>> OnErrorAsync<T>(
        this Task<Result<T>> result,
        IAsyncCommand<IError> command)
        where T : notnull
    {
        Result<T> awaited = await result;
        return await awaited.OnErrorAsync(command);
    }
}