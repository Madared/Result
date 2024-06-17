using ResultAndOption.Commands;
using ResultAndOption.Errors;
using ResultAndOption.Mappers;

namespace ResultAndOption.Results.GenericResultExtensions;

public static class SimpleNoLeakAsync
{
    public static async Task<Result<T>> MapAsync<T>(this Task<Result> result, IMapper<T> mapper) where T : notnull
    {
        Result awaited = await result;
        return awaited.Map(mapper);
    }

    public static async Task<Result<T>> MapAsync<T>(this Task<Result> result, IAsyncMapper<T> mapper) where T : notnull
    {
        Result awaited = await result;
        return await awaited.MapAsync(mapper);
    }

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