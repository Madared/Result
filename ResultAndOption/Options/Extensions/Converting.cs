namespace ResultAndOption.Options.Extensions;

public static class Converting {
    /// <summary>
    ///     Returns the value inside the option, in case it is empty returns the non-null value passed in
    /// </summary>
    /// <param name="defaultValue">Value to return in case option is empty</param>
    /// <returns></returns>
    public static T Or<T>(this Option<T> option, T defaultValue) where T : notnull => option.IsNone() ? defaultValue : option.Data;

    public static Option<T> OrNullable<T>(this Option<T> option, T? replacement) where T : notnull => option.IsNone() ? Option<T>.Maybe(replacement) : option;

    /// <summary>
    ///     Returns this option if it isnt empty, otherwise returns the passed in option
    /// </summary>
    /// <param name="replacement">replacement option</param>
    /// <returns></returns>
    public static Option<T> OrOption<T>(this Option<T> option, Option<T> replacement) where T : notnull => option.IsNone() ? replacement : option;

    /// <summary>
    ///     Generates an option type based on a null reference type
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Option<T> ToOption<T>(this T? data) where T : notnull => Option<T>.Maybe(data);
}