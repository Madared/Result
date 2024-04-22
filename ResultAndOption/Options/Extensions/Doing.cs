namespace ResultAndOption.Options.Extensions;

public static class Doing {
    /// <summary>
    ///     Performs the action in case the option is not empty, and returns this option
    /// </summary>
    /// <param name="option"></param>
    /// <param name="action">Action to perform</param>
    /// <returns></returns>
    public static Option<T> Do<T>(this Option<T> option, Action<T> action) where T : notnull {
        if (option.IsSome()) action(option.Data);
        return option;
    } 
}