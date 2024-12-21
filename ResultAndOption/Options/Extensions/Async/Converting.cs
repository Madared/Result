namespace ResultAndOption.Options.Extensions.Async;

/// <summary>
/// Contains all methods for converting Async Options
/// </summary>
public static class Converting
{
    /// <summary>
    /// Converts any Task of a nullable reference to a Task of Option
    /// </summary>
    /// <param name="nullable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Option<T>> ToOptionAsync<T>(this Task<T?> nullable) where T : notnull
    {
        T? data = await nullable;
        return data.ToOption();
    }

    /// <summary>
    /// Provides a fallback for an async option in case it is empty.
    /// </summary>
    /// <param name="asyncOption"></param>
    /// <param name="fallback"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<T> OrAsync<T>(this Task<Option<T>> asyncOption, T fallback) where T : notnull
    {
        Option<T> option = await asyncOption;
        return option.IsSome() ? option.Data : fallback;
    }

    /// <summary>
    /// Returns the existing option if it is not empty otherwise turns the replacement value into an option and returns it
    /// </summary>
    /// <param name="asyncOption"></param>
    /// <param name="replacement"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Option<T>> OrNullableAsync<T>(this Task<Option<T>> asyncOption, T? replacement) where T : notnull
    {
        Option<T> option = await asyncOption;
        return option.IsNone() ? Option<T>.Maybe(replacement) : option;
    }

    /// <summary>
    /// Returns this option if it is not empty, otherwise returns the passed in option
    /// </summary>
    /// <param name="asyncOption"></param>
    /// <param name="replacement">replacement option</param>
    /// <returns></returns>
    public static async Task<Option<T>> OrOption<T>(this Task<Option<T>> asyncOption, Option<T> replacement) where T : notnull
    {
        Option<T> option = await asyncOption;
        return option.IsNone() ? replacement : option;
    }
}