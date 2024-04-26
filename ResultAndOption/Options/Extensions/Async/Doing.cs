namespace ResultAndOption.Options.Extensions.Async;

public static class Doing
{
    public delegate Task<Option<T>> OptionAsyncActionWithInput<T>(T data, CancellationToken? token = null) where T : notnull;
    public delegate Task<Option<T>> OptionAsyncAction<T>(CancellationToken? token = null) where T : notnull;

    /// <summary>
    /// Awaits for the option and invokes the action if the option is not empty
    /// </summary>
    /// <param name="option"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Option<T>> DoAsync<T>(this Task<Option<T>> option, Action<T> action) where T : notnull
    {
        Option<T> originalOption = await option;
        return originalOption.Do(action);
    }

    public static async Task<Option<T>> DoAsync<T>(this Task<Option<T>> option, Action action) where T : notnull
    {
        Option<T> originalOption = await option;
        return originalOption.Do(action);
    }

    public static async Task<Option<T>> DoAsync<T>(
        this Option<T> option,
        OptionAsyncActionWithInput<T> actionWithInput,
        CancellationToken? token = null) where T : notnull
    {
        if (option.IsSome())
        {
            await actionWithInput(option.Data, token);
        }

        return option;
    }

    public static async Task<Option<T>> DoAsync<T>(
        this Task<Option<T>> option,
        OptionAsyncActionWithInput<T> actionWithInput,
        CancellationToken? token = null) where T : notnull
    {
        Option<T> originalOption = await option;
        if (originalOption.IsSome())
        {
            await actionWithInput(originalOption.Data, token);
        }

        return originalOption;
    }


    public static async Task<Option<T>> DoAsync<T>(
        this Option<T> option,
        OptionAsyncAction<T> action,
        CancellationToken? token = null) where T : notnull
    {
        if (option.IsSome())
        {
            await action(token);
        }

        return option;
    }

    public static async Task<Option<T>> DoAsync<T>(
        this Task<Option<T>> option,
        OptionAsyncAction<T> action,
        CancellationToken? token = null) where T : notnull
    {
        Option<T> originalOption = await option;
        if (originalOption.IsSome())
        {
            await action(token);
        }

        return originalOption;
    }
}