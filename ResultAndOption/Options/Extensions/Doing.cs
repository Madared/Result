using ResultAndOption.Results;

namespace ResultAndOption.Options.Extensions;

/// <summary>
/// Contains all methods to call Actions on an Option
/// </summary>
public static class Doing
{
    /// <summary>
    /// Performs the action in case the option is not empty, and returns this option
    /// </summary>
    /// <param name="option"></param>
    /// <param name="action">Action to perform</param>
    /// <returns></returns>
    public static Option<T> Do<T>(this Option<T> option, Action<T> action) where T : notnull
    {
        if (option.IsSome()) action(option.Data);
        return option;
    }

    /// <summary>
    /// Performs action in case option is not empty and returns this option
    /// </summary>
    /// <param name="option"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Option<T> Do<T>(this Option<T> option, Action action) where T : notnull
    {
        if (option.IsSome()) action();
        return option;
    }
}