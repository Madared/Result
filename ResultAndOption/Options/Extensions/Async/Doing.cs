namespace ResultAndOption.Options.Extensions.Async;

public static class Doing {
    /// <summary>
    /// Awaits for the option and invokes the action if the option is not empty
    /// </summary>
    /// <param name="option"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Option<T>> DoAsync<T>(this Task<Option<T>> option, Action<T> action) where T : notnull {
        Option<T> originalOption = await option;
        return originalOption.Do(action);
    }
}