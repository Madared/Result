namespace ResultAndOption.Options.Extensions;

/// <summary>
/// Contains all methods to map an option
/// </summary>
public static class Mapping
{
    /// <summary>
    /// Maps the option into the result of the function wrapped in an option, if the options is empty
    /// it foregoes calling the map function and just returns an empty option
    /// </summary>
    /// <param name="option"></param>
    /// <param name="function">mapping function</param>
    /// <returns></returns>
    public static Option<TOut> Map<T, TOut>(this Option<T> option, Func<T, TOut> function)
        where T : notnull where TOut : notnull => option.IsNone()
        ? Option<TOut>.None()
        : Option<TOut>.Some(function(option.Data));

    /// <summary>
    /// Maps the option into the new Option type returned by the function, foregoes calling the mapper if the option is
    /// empty and just returns a new empty option of the new type
    /// </summary>
    /// <param name="option"></param>
    /// <param name="function">mapping function</param>
    /// <returns></returns>
    public static Option<TOut> Map<T, TOut>(this Option<T> option, Func<T, Option<TOut>> function)
        where T : notnull where TOut : notnull => option.IsNone()
        ? Option<TOut>.None()
        : function(option.Data);
}