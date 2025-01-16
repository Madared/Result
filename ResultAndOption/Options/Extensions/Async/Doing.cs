namespace ResultAndOption.Options.Extensions.Async;

/// <summary>
/// Extensions for asynchronous actions and commands on options
/// </summary>
public static class Doing
{
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

    /// <summary>
    /// Performs action if Task of option is populated
    /// </summary>
    /// <param name="option">The option to check</param>
    /// <param name="action">The action to perform</param>
    /// <typeparam name="T">The type of the option</typeparam>
    /// <returns></returns>
    public static async Task<Option<T>> DoAsync<T>(this Task<Option<T>> option, Action action) where T : notnull
    {
        Option<T> originalOption = await option;
        return originalOption.Do(action);
    }

    /// <summary>
    /// Performs an asynchronous action with data in option if it is populated
    /// </summary>
    /// <param name="option">Option to check</param>
    /// <param name="actionWithInput">Action to perform</param>
    /// <param name="token">Cancellation token</param>
    /// <typeparam name="T">Type of option</typeparam>
    /// <returns></returns>
    public static async Task<Option<T>> DoAsync<T>(
        this Option<T> option,
        Func<T, CancellationToken?, Task> actionWithInput,
        CancellationToken? token = null) where T : notnull
    {
        if (option.IsSome())
        {
            await actionWithInput(option.Data, token);
        }

        return option;
    }

    /// <summary>
    /// Performs asynchronous action on asynchronous option with its value if populated
    /// </summary>
    /// <param name="option">Option to check</param>
    /// <param name="actionWithInput">Action to perform</param>
    /// <param name="token">Cancellation token</param>
    /// <typeparam name="T">Type of option</typeparam>
    /// <returns></returns>
    public static async Task<Option<T>> DoAsync<T>(
        this Task<Option<T>> option,
        Func<T, CancellationToken?, Task> actionWithInput,
        CancellationToken? token = null) where T : notnull
    {
        Option<T> originalOption = await option;
        if (originalOption.IsSome())
        {
            await actionWithInput(originalOption.Data, token);
        }

        return originalOption;
    }


    /// <summary>
    /// Performs asynchronous action if option is populated
    /// </summary>
    /// <param name="option">Option to check</param>
    /// <param name="action">Action to perform</param>
    /// <param name="token">Cancellation token</param>
    /// <typeparam name="T">Type of option</typeparam>
    /// <returns></returns>
    public static async Task<Option<T>> DoAsync<T>(
        this Option<T> option,
        Func<CancellationToken?, Task> action,
        CancellationToken? token = null) where T : notnull
    {
        if (option.IsSome())
        {
            await action(token);
        }

        return option;
    }

    /// <summary>
    /// Performs Asynchronous action if asynchronous option is populated
    /// </summary>
    /// <param name="option">Option to check</param>
    /// <param name="action">Action to perform</param>
    /// <param name="token">Cancellation token</param>
    /// <typeparam name="T">Type of option</typeparam>
    /// <returns></returns>
    public static async Task<Option<T>> DoAsync<T>(
        this Task<Option<T>> option,
        Func<CancellationToken?, Task> action,
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