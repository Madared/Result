using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;
using ResultAndOption.Results.GenericResultExtensions.Async;
using ResultAndOption.Results.Mappers;

namespace ResultAndOption.Results.GenericResultExtensions;

public static class GenericNoLeakAsync
{
    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, IMapper<TOut> mapper)
        where T : notnull where TOut : notnull
    {
        Result<T> awaited = await result;
        return awaited.Map(mapper);
    }

    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, IAsyncMapper<TOut> mapper)
        where T : notnull where TOut : notnull
    {
        Result<T> awaited = await result;
        return await awaited.MapAsync(mapper);
    }

    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, IMapper<T, TOut> mapper)
        where T : notnull where TOut : notnull
    {
        Result<T> awaited = await result;
        return awaited.Map(mapper);
    }

    public static async Task<Result<TOut>> MapAsync<T, TOut>(this Task<Result<T>> result, IAsyncMapper<T, TOut> mapper)
        where T : notnull where TOut : notnull
    {
        Result<T> awaited = await result;
        return await awaited.MapAsync(mapper);
    }

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