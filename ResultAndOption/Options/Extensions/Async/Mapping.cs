namespace ResultAndOption.Options.Extensions.Async;

/// <summary>
/// Contains all MapAsync methods
/// </summary>
public static class Mapping {
    /// <summary>
    /// Awaits for the option and calls the specific Map method
    /// </summary>
    /// <param name="option"></param>
    /// <param name="asyncMapper">asynchronous mapping function</param>
    /// <returns></returns>
    public static async Task<Option<TOut>> MapAsync<T, TOut>(this Option<T> option, Func<T, Task<TOut?>> asyncMapper) where T : notnull where TOut : notnull {
        if (option.IsNone()) return Option<TOut>.None();

        TOut? mapResult = await asyncMapper(option.Data);
        return mapResult is null ? Option<TOut>.None() : Option<TOut>.Some(mapResult);
    }

    /// <summary>
    /// Awaits for the option and calls the specific Map method
    /// </summary>
    /// <param name="option"></param>
    /// <param name="asyncMapper">asynchronous mapping function</param>
    /// <returns></returns>
    public static async Task<Option<TOut>> MapAsync<T, TOut>(this Option<T> option, Func<T, Task<Option<TOut>>> asyncMapper) where T : notnull where TOut : notnull {
        if (option.IsNone()) return Option<TOut>.None();
        return await asyncMapper(option.Data);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="option"></param>
    /// <param name="mapper"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Option<TOut>> MapAsync<T, TOut>(this Task<Option<T>> option, Func<T, TOut> mapper) where T : notnull where TOut : notnull {
        Option<T> originalOption = await option;
        return originalOption.Map(mapper);
    }

    /// <summary>
    /// Awaits for the Option and then calls the specific Map method
    /// </summary>
    /// <param name="option"></param>
    /// <param name="asyncMapper"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Option<TOut>> MapAsync<T, TOut>(this Task<Option<T>> option, Func<T, Task<TOut?>> asyncMapper) where T : notnull where TOut : notnull {
        Option<T> originalOption = await option;
        return await originalOption.MapAsync(asyncMapper);
    }

    /// <summary>
    /// Awaits for the option and calls the specific Map method
    /// </summary>
    /// <param name="option"></param>
    /// <param name="mapper"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Option<TOut>> MapAsync<T, TOut>(this Task<Option<T>> option, Func<T, Option<TOut>> mapper) where T : notnull where TOut : notnull {
        Option<T> originalOption = await option;
        return originalOption.Map(mapper);
    }

    /// <summary>
    /// Awaits for the option and calls the specific Map method
    /// </summary>
    /// <param name="option"></param>
    /// <param name="asyncMapper"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<Option<TOut>> MapAsync<T, TOut>(this Task<Option<T>> option, Func<T, Task<Option<TOut>>> asyncMapper) where T : notnull where TOut : notnull {
        Option<T> originalOption = await option;
        return await originalOption.MapAsync(asyncMapper);
    }
}