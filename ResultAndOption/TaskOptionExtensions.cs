namespace Results;

public static class TaskOptionExtensions {
    public static async Task<Option<TOut>> MapAsync<T, TOut>(this Task<Option<T>> option, Func<T, TOut> mapper) where T : notnull where TOut : notnull {
        Option<T> originalOption = await option;
        return originalOption.Map(mapper);
    }

    public static async Task<Option<TOut>> MapAsync<T, TOut>(this Task<Option<T>> option, Func<T, Task<TOut?>> asyncMapper) where T : notnull where TOut : notnull {
        Option<T> originalOption = await option;
        return await originalOption.MapAsync(asyncMapper);
    }

    public static async Task<Option<TOut>> MapAsync<T, TOut>(this Task<Option<T>> option, Func<T, Option<TOut>> mapper) where T : notnull where TOut : notnull {
        Option<T> originalOption = await option;
        return originalOption.Map(mapper);
    }

    public static async Task<Option<TOut>> MapAsync<T, TOut>(this Task<Option<T>> option, Func<T, Task<Option<TOut>>> asyncMapper) where T : notnull where TOut : notnull {
        Option<T> originalOption = await option;
        return await originalOption.MapAsync(asyncMapper);
    }

    public static async Task<Option<T>> ToOptionAsync<T>(this Task<T?> nullable) where T : notnull {
        T? data = await nullable;
        return data.ToOption();
    }

    public static async Task<Option<T>> UseDataAsync<T>(this Task<Option<T>> option, Action<T> action) where T : notnull {
        Option<T> originalOption = await option;
        return originalOption.Do(action);
    }
}