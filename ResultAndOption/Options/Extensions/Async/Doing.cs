namespace ResultAndOption.Options.Extensions.Async;

public static class Doing {
    public static async Task<Option<T>> DoAsync<T>(this Task<Option<T>> option, Action<T> action) where T : notnull {
        Option<T> originalOption = await option;
        return originalOption.Do(action);
    }
}