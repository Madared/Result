namespace ResultAndOption.Options.Extensions;

/// <summary>
/// Contains all Option conversion methods
/// </summary>
public static class Converting
{
    /// <summary>
    ///     Returns the value inside the option, in case it is empty returns the non-null value passed in
    /// </summary>
    /// <param name="option"></param>
    /// <param name="defaultValue">Value to return in case option is empty</param>
    /// <returns></returns>
    public static T Or<T>(this Option<T> option, T defaultValue) where T : notnull =>
        option.IsNone() ? defaultValue : option.Data;

    /// <summary>
    /// Returns the existing option if it is not empty otherwise turns the replacement value into an option and returns it
    /// </summary>
    /// <param name="option"></param>
    /// <param name="replacement"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Option<T> OrNullable<T>(this Option<T> option, T? replacement) where T : notnull =>
        option.IsNone() ? Option<T>.Maybe(replacement) : option;

    /// <summary>
    /// Returns this option if it is not empty, otherwise returns the passed in option
    /// </summary>
    /// <param name="option"></param>
    /// <param name="replacement">replacement option</param>
    /// <returns></returns>
    public static Option<T> OrOption<T>(this Option<T> option, Option<T> replacement) where T : notnull =>
        option.IsNone() ? replacement : option;

    /// <summary>
    /// Generates an option type based on a null reference type
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Option<T> ToOption<T>(this T? data) where T : notnull => Option<T>.Maybe(data);
}