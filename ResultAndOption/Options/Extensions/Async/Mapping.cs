namespace ResultAndOption.Options.Extensions.Async;

public static class Mapping {
   
    /// <summary>
    ///     Maps the Option by implicitly awaiting the asynchronous mapping function, foregoes calling the function if
    ///     the option is empty
    /// </summary>
    /// <param name="asyncMapper">asynchronous mapping function</param>
    /// <returns></returns>
    public static async Task<Option<TOut>> MapAsync<T, TOut>(this Option<T> option, Func<T, Task<TOut?>> asyncMapper) where T : notnull where TOut : notnull {
        if (option.IsNone()) return Option<TOut>.None();

        var mapResult = await asyncMapper(option.Data);
        return mapResult is null ? Option<TOut>.None() : Option<TOut>.Some(mapResult);
    }

    /// <summary>
    ///     Maps the Option by implicitly awaiting the asynchronous mapping function, foregoes calling the function if
    ///     the option is empty
    /// </summary>
    /// <param name="asyncMapper">asynchronous mapping function</param>
    /// <returns></returns>
    public static async Task<Option<TOut>> MapAsync<T, TOut>(this Option<T> option, Func<T, Task<Option<TOut>>> asyncMapper) where T : notnull where TOut : notnull {
        if (option.IsNone()) return Option<TOut>.None();
        return await asyncMapper(option.Data);
    } 
    
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
}